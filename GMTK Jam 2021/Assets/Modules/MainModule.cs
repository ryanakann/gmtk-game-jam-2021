using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller))]
[RequireComponent(typeof(Rigidbody2D))]
public class MainModule : Module {
    HashSet<Module> modules;//modules under my control

    List<Deliverable> deliverables = new List<Deliverable>();

    protected void Start() {
        modules = new HashSet<Module>();
        mainModule = gameObject;
        AddModule(this);
        rb = GetComponent<Rigidbody2D>();
        foreach (var t in GetComponentsInChildren<Thruster>())
        {
            t.Activate();
            t.updateButtons();
        }
    }
    
    protected override void Update() {
        base.Update();
    }
    
    public void AddModule(Module m) {
        modules.Add(m);
        foreach (var mod in modules)
        {
            foreach(Thruster thruster in mod.GetComponents<Thruster>())
            {
                thruster.updateButtons();
            }
        }
    }
    
    public bool RemoveModule(Module m) {
        bool result = modules.Remove(m);
        foreach (var mod in modules)
        {
            foreach (Thruster thruster in mod.GetComponents<Thruster>())
            {
                thruster.updateButtons();
            }
        }
        return result;
    }

    public void AddDeliverable(Deliverable d)
    {
        deliverables.Add(d);
    }

    public bool RemoveDeliverable(Deliverable d)
    {
        return deliverables.Remove(d);
    }

    public void PropagateJostle(float impact)
    {
        foreach(var d in deliverables)
        {
            d.Jostle(impact);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Module module = collision.otherCollider.GetComponentInParent<Module>();
        print("module is: " + module);

        if (module)
        {
            module.OnCollision(collision);
        }
    }
}
