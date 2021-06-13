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
        Vector2 forward = mainModule.transform.up; // 0, 1, 0, 1
        return Vector2.Dot(getFiringDirection(), forward) > epsilon;
    }
    public bool shouldFireBackward() {
        Vector2 forward = -mainModule.transform.up; // 0, -1 downward facing module   0, -1,  0, 1
        return Vector2.Dot(getFiringDirection(), forward) > epsilon;
    }
    public bool shouldFireCounterclockwise() {
        Vector3 center = mainModule.GetComponent<MainModule>().rb.centerOfMass;
        Vector2 diff = gameObject.transform.position - center;
        Vector2 perp = Vector2.Perpendicular(diff);
        return Vector2.Dot(getFiringDirection(), perp) < -epsilon;
    }
    public bool shouldFireClockwise() {
        Vector3 center = mainModule.GetComponent<MainModule>().rb.centerOfMass;
        Vector2 diff = gameObject.transform.position - center;
        Vector2 perp = Vector2.Perpendicular(diff);
        return Vector2.Dot(getFiringDirection(), perp) > epsilon;
    }

    //TODO: this can be done whenever modules are added/destroyed, not necessary on every update
    public void updateButtons() {
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
            AssignButton(KeyCode.D);
        } else {
            UnassignButton(KeyCode.D);
        }
        if (shouldFireClockwise()) {
            AssignButton(KeyCode.A);
        } else {
            UnassignButton(KeyCode.A);
        }
    }

    protected override void Update() {
        if (isDetached)
        {
            return;
        }
        //updateButtons();
        firing = false;
        //TODO: set flame sprite inactive
        base.Update();//will determine if still firing
        if (firing) {
            Debug.DrawRay(transform.position, getFiringDirection() * strength);
            mainModule.GetComponent<MainModule>().rb.AddForceAtPosition(getFiringDirection() * strength * Time.deltaTime, transform.position);
        }
    }
    public override void OnButtonHeld() {
        firing = true;
        //TODO: set flame sprite active
    }
}
