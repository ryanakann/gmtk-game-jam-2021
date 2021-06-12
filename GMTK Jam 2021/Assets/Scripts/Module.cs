using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour {
    public class Port {
        public Module parent;
        public Module child;
    }

    KeyCode button;
    List<Port> ports;
    public float health;
    public float maxHealth = 10f;

    private void Start() {
        health = maxHealth;
    }

    public void AssignButton(KeyCode key) {
        button = key;
    }

    public void AttachChildAtPort(Module child, int portIndex) {
        ports[portIndex].child = child;
        //TODO: physically attach the module's gameobject
    }

    public Module DetachAtPort(int portIndex) {
        Module result = ports[portIndex].child;
        //TODO: physically detach the module's gameobject
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

    void Update() {
        if (Input.GetKeyDown(button)) {
            OnButtonDown();
        }
        if (Input.GetKey(button)) {
            OnButtonHeld();
        }
        if (Input.GetKeyUp(button)) {
            OnButtonUp();
        }
        if (health <= 0) {
            Die();
        }
    }

    public virtual void Die() {
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
}
