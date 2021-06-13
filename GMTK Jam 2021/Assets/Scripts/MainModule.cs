using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller))]
public class MainModule : Module {
    HashSet<Module> modules;//modules under my control
    public Vector2 centerOfMass { get; private set; }

    List<Deliverable> deliverables = new List<Deliverable>();

    protected override void Start() {
        base.Start();
        modules = new HashSet<Module>();
        mainModule = gameObject;
        AddModule(this);
    }
    
    protected override void Update() {
        base.Update();
        centerOfMass = getCenterOfMass();
    }
    
    public void AddModule(Module m) {
        modules.Add(m);
        foreach (var mod in modules)
        {
            if (mod is Thruster)
            {
                ((Thruster)mod).updateButtons();
            }
        }
    }
    
    public bool RemoveModule(Module m) {
        bool result = modules.Remove(m);
        foreach (var mod in modules)
        {
            if (mod is Thruster)
            {
                ((Thruster)mod).updateButtons();
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

    public Vector2 getCenterOfMass() {
        Vector2 weightedAverage = transform.position * GetComponent<Rigidbody2D>().mass;
        foreach (Module m in modules) {
            if (m == this) {
                continue;
            }
            weightedAverage += (Vector2)m.transform.position * m.GetComponent<Rigidbody2D>().mass;
        }
        return weightedAverage;
    }

    public void PropagateJostle(float impact)
    {
        foreach(var d in deliverables)
        {
            d.Jostle(impact);
        }
    }
}
