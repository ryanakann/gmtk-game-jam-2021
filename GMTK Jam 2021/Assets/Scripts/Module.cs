using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour {
    public class Port {
        public Module parent;
        public Module child;
    }

    HashSet<KeyCode> buttons;
    List<Port> ports;
    protected GameObject mainModule;//which ship am I attached to?
    public float health;
    public float maxHealth = 10f;

    private void Start() {
        health = maxHealth;
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
        //TODO: physically attach the module's gameobject
    }

    public Module DetachAtPort(int portIndex) {
        Module result = ports[portIndex].child;
        //TODO: physically detach the module's gameobject
        ports[portIndex].child.mainModule = null;
        ports[portIndex].child = null;
        return result;
    }
    public Module GetModuleAtPort(int portIndex) {
        Module result = ports[portIndex].child;
        if (result == this) {//attached to parent by this port
            return ports[portIndex].parent;
        }
        return result;
    }

    protected virtual void Update() {
        bool buttonHeld = false;
        foreach (KeyCode button in buttons) {
            if (Input.GetKey(button)) {
                buttonHeld = true;
            }
        }
        if (buttonHeld) {
            OnButtonHeld();
        }
        if (health <= 0) {
            Die();
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

    public void Damage(float damage_amount) {
        health -= damage_amount;

        if (health <= 0) {
            Die();
        }
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
}
