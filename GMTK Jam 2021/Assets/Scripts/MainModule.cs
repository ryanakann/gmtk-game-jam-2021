using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller))]
public class MainModule : Module {
    HashSet<Module> modules;//modules under my control
    public Vector2 centerOfMass { get; private set; }

    List<Deliverable> deliverables;

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
    }
    public bool RemoveModule(Module m) {
        return modules.Remove(m);
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
