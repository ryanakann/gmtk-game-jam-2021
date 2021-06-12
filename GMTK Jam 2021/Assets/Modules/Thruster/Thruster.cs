using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Thruster : Module {
    [SerializeField]
    float strength = 30f;

    float epsilon = 0.0001f;
    bool firing;

    public Vector2 getFiringDirection() {
        return transform.up;
    }
    public bool shouldFireForward() {
        Vector2 forward = mainModule.transform.up;
        return Vector2.Dot(getFiringDirection(), forward) < -epsilon;
    }
    public bool shouldFireBackward() {
        Vector2 forward = mainModule.transform.up;
        return Vector2.Dot(getFiringDirection(), forward) > epsilon;
    }
    public bool shouldFireCounterclockwise() {
        Vector3 center = mainModule.GetComponent<MainModule>().getCenterOfMass();
        Vector2 diff = gameObject.transform.position - center;
        Vector2 perp = Vector2.Perpendicular(diff);
        return Vector2.Dot(getFiringDirection(), perp) < -epsilon;
    }
    public bool shouldFireClockwise() {
        Vector3 center = mainModule.GetComponent<MainModule>().getCenterOfMass();
        Vector2 diff = gameObject.transform.position - center;
        Vector2 perp = Vector2.Perpendicular(diff);
        return Vector2.Dot(getFiringDirection(), perp) > epsilon;
    }

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
        //TODO: set flame sprite inactive
        base.Update();//will determine if still firing
        if (firing) {
            GetComponent<Rigidbody2D>().AddForce(getFiringDirection() * strength * Time.deltaTime);
        }
    }
    public override void OnButtonHeld() {
        firing = true;
        //TODO: set flame sprite active
    }
}
