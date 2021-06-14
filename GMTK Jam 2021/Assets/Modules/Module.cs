using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour {
    HashSet<KeyCode> buttons;
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

    float disabled_timer = 0f;
    protected bool is_disabled = false;

    public bool isDetached;

    protected virtual void Awake() {
        children = new List<Module>();
        buttons = new HashSet<KeyCode>();
        health = max_health;
    }

    public void AssignButton(KeyCode key) {
        buttons.Add(key);
    }

    public bool UnassignButton(KeyCode key) {
        return buttons.Remove(key);
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

        mainModule.GetComponent<MainModule>().AddModule(this);
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
    }
    public List<Module> GetChildModules() {
        return children;
    }

    protected virtual void Update() {
        if (health <= 0) {
            Die();
        }

        bool buttonHeld = false;

        if (is_disabled) {
            disabled_timer -= Time.deltaTime;

            if (disabled_timer <= 0f)
                is_disabled = false;
            else
                return;
        }

        foreach (KeyCode button in buttons) {
            if (mainModule.GetComponent<Controller>().GetKey(button)) {
                buttonHeld = true;
            }
        }
        if (buttonHeld) {
            OnButtonHeld();
        }
    }
    public virtual void Die() {
        Detach();
        Destroy(gameObject);
    }

    public virtual void OnButtonDown() {
        //no thoughts head empty
    }

    public virtual void OnButtonHeld() {
        //no thoughts head empty
    }

    public virtual void OnButtonUp() {
        //no thoughts head empty
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

    public virtual void Disable(float seconds_disabled) {
        is_disabled = true;
        disabled_timer = seconds_disabled;
    }

    public void Explode(float power, float radius) {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D collider in colliders) {
            Rigidbody2D rb = collider.attachedRigidbody;

            if (rb != null) {
                rb.AddExplosionForce(power, transform.position, radius, position:collider.transform.position);
            }

            Module module = collider.GetComponent<Module>();
            if (module != null) {
                Vector2 diff = collider.transform.position - transform.position;
                float distance = diff.magnitude;
                module.Damage(power / distance);
            }
        }
    }

    public virtual void OnCollision(Collision2D collision) {
        if (collision.gameObject.tag != "projectile") {
            float impulse = collision.relativeVelocity.magnitude;
            if (impulse > impact_velocity_threshold)
            {
                print("oh boy we do be gettin damage from the impulse: " + impulse);
                Damage(impulse * impact_damage_coefficient);
            }
            if (mainModule != this)
            {
                mainModule.GetComponent<MainModule>().PropagateJostle(impulse);
            }
        }
    }
}