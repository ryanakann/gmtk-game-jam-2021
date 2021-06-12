using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Thruster : Module {
    float epsilon = 0.0001f;
    public Vector2 getFiringDirection() {
        return -transform.up;
    }
    public bool shouldFireForward() {
        Vector2 forward = mainModule.transform.up;
        return Vector2.Dot(getFiringDirection(), forward) > epsilon;
    }
    public bool shouldFireBackward() {
        Vector2 forward = mainModule.transform.up;
        return Vector2.Dot(getFiringDirection(), forward) < -epsilon;
    }
    public bool shouldFireCounterclockwise() {
        return true;
    }
    public bool shouldFireClockwise() {
        return true;
    }
    bool firing;

    //TODO: this can be done whenever modules are added/destroyed, not necessary on every update
    void updateButtons() {
        if (shouldFireForward()) {
            AssignButton(KeyCode.W);
        } else {
            UnassignButton(KeyCode.W);
        }
        if (shouldFireBackward()) {
            AssignButton(KeyCode.S);
        } else {
            UnassignButton(KeyCode.S);
        }
        if (shouldFireCounterclockwise()) {
            AssignButton(KeyCode.A);
        } else {
            UnassignButton(KeyCode.A);
        }
        if (shouldFireClockwise()) {
            AssignButton(KeyCode.D);
        } else {
            UnassignButton(KeyCode.D);
        }
    }

    protected override void Update() {
        updateButtons();
        firing = false;
        base.Update();//will determine if still firing
        if (firing) {
            Vector2 fireDirection = -transform.up;
        }
    }
    public override void OnButtonHeld() {
        firing = true;
    }
}
