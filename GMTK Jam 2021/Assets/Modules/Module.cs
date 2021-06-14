using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour {
    protected List<Module> children;
    protected Module parent;
    protected GameObject mainModule;//which ship am I attached to?

    [SerializeField]
    float max_health;
    float health;

    [SerializeField]
    float impact_velocity_threshold;

    [SerializeField]
    float impact_damage_coefficient;

    ModuleBehavior[] behaviors;

    float disabled_timer = 0f;
    protected bool is_disabled = false;

    public bool isDetached;

    [HideInInspector]
    public Rigidbody2D rb;

    protected virtual void Awake() {
        children = new List<Module>();
        health = max_health;

        rb = GetComponent<Rigidbody2D>();

        behaviors = GetComponents<ModuleBehavior>();
    }

    public virtual void AddChild(Module child)
    {
        children.Add(child);
    }

    public virtual void SetParent(Module parent, Transform pivot) {
        isDetached = false;
        parent.AddChild(this);
        this.parent = parent;
        mainModule = parent.mainModule;
        //physically attach the module's gameobject
        transform.position = pivot.position;
        transform.up = -pivot.up;
        if (pivot != transform)
            transform.parent = mainModule.transform;

        MainModule mainModuleComponent = mainModule.GetComponent<MainModule>();

        foreach (ModuleBehavior behavior in behaviors)
        {
            behavior.SetMainModule(mainModuleComponent);
        }

        mainModuleComponent.AddModule(this);
        Destroy(rb);
        ActivateBehaviors();
    }

    public void Detach() {
        isDetached = true;
        if (mainModule == this)
            return;
        mainModule.GetComponent<MainModule>().RemoveModule(this);
        if (parent != null)
            parent.children.Remove(this);
        mainModule = null;
        parent = null;
        transform.parent = null;
        rb = gameObject.AddComponent<Rigidbody2D>();
        DeactivateBehaviors();
    }
    public List<Module> GetChildModules() {
        return children;
    }

    protected virtual void Update() {
        if (is_disabled)
        {
            disabled_timer -= Time.deltaTime;

            if (disabled_timer <= 0f)
            {
                is_disabled = false;
                ActivateBehaviors();
            }
        }

        if (health <= 0) {
            Die();
        }
    }
    public virtual void Die() {
        foreach (ModuleBehavior behavior in behaviors)
        {
            behavior.Die();
        }

        Detach();
        Destroy(gameObject);
    }

    public virtual void Heal(float heal_amount) {
        health += heal_amount;

        if (health >= max_health)
            health = max_health;
    }

    public virtual void Damage(float damage_amount) {
        health -= damage_amount;

        if (health <= 0) {
            Die();
        }
    }

    public virtual void Disable(float seconds_disabled)
    {
        is_disabled = true;
        disabled_timer = seconds_disabled;
        DeactivateBehaviors();
    }

    void ActivateBehaviors()
    {
        foreach (ModuleBehavior b in behaviors)
        {
            b.Activate();
        }
    }

    void DeactivateBehaviors()
    {
        foreach (ModuleBehavior b in behaviors)
        {
            b.Deactivate();
        }
    }

    public void Explode(float power, float radius) {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        print("it is explosion time");
        
        foreach (Collider2D collider in colliders) {

            print("collider: " + collider);

            if (collider.gameObject == gameObject)
                continue;
            
            Rigidbody2D rb = collider.attachedRigidbody;

            if (rb != null) {
                rb.AddExplosionForce(power, transform.position, radius, position:collider.transform.position);
            }

            Module module = collider.GetComponentInParent<Module>();
            if (module != null) {
                Vector2 diff = collider.transform.position - transform.position;
                float distance = diff.magnitude;
                float damage = power / distance;
                module.Damage(damage);
                print("doing explosion damage: " + damage);
            }
        }
        

        print("unity you suck");

        Die();
    }

    public virtual void OnCollision(Collision2D collision) {
        if (collision.gameObject.tag != "projectile") {
            float impulse = collision.relativeVelocity.magnitude;
            if (mainModule && mainModule != this)
            {
                mainModule.GetComponent<MainModule>().PropagateJostle(impulse);
            }
            if (impulse > impact_velocity_threshold)
            {
                print("oh boy we do be gettin damage from the impulse: " + impulse);
                Damage(impulse * impact_damage_coefficient);
            }
        }
    }
}
