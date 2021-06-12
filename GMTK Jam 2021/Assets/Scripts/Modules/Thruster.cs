using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : Module {
    bool active;
    protected override void Update() {
        base.Update();
        Vector2 fireDirection = -transform.up;
    }
}
