using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainModule : Module {
    HashSet<Module> modules;//modules under my control
    public void AddModule(Module m) {
        modules.Add(m);
    }
    public bool RemoveModule(Module m) {
        return modules.Remove(m);
    }
    public Vector2 getCenterOfMass() {
        //TODO: calculate center of mass of all modules
        return transform.position;
    }
}
