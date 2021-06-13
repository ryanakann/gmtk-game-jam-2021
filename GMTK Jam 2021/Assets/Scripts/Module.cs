using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Module : MonoBehaviour {
    public class Port {
        public Module parent;
        public Module child;
    }

    HashSet<KeyCode> buttons;
    List<Port> ports;
    protected GameObject mainModule;//which ship am I attached to?

    [SerializeField]
    float max_health;
    float health;

    [SerializeField]
    float impact_velocity_threshold;

    [SerializeField]
    float impact_damage_scaler;

    float disabled_timer = 0f;
    protected bool is_disabled = false;

    public bool isDetached;

    protected virtual void Start() {
        buttons = new HashSet<KeyCode>();
        ports = new List<Port>();
        health = max_health;
    }

    public void AssignButton(KeyCode key) {
        buttons.Add(key);
    }

    public bool UnassignButton(KeyCode key) {
        return buttons.Remove(key);
    }

    public void AttachChildAtPort(Module child, int portIndex) {
        ports[portIndex].child = child;
        child.mainModule = mainModule;
        mainModule.GetComponent<MainModule>().AddModule(child);
        //TODO: physically attach the module's gameobject
    }

    public Module DetachAtPort(int portIndex) {
        Module removed = ports[portIndex].child;
        mainModule.GetComponent<MainModule>().RemoveModule(removed);
        //TODO: physically detach the module's gameobject
        ports[portIndex].child.mainModule = null;
        ports[portIndex].child = null;
        return removed;
    }
    public Module GetModuleAtPort(int portIndex) {
        Module result = ports[portIndex].child;
        if (result == this) {//attached to parent by this port
            return ports[portIndex].parent;
        }
        return result;
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
        Destroy(gameObject);
        //TODO: destroy child modules?
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
                rb.AddExplosionForce(power, transform.position, radius);
            }

            Module module = collider.GetComponent<Module>();
            if (module != null) {
                Vector2 diff = collider.transform.position - transform.position;
                float distance = diff.magnitude;
                module.Damage(power / distance);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "projectile")
        {
            float impulse = collision.contacts[0].normalImpulse;
            if (impulse > impact_velocity_threshold)
            {
                Damage(impulse * impact_damage_scaler);
            }
        }
    }
}
