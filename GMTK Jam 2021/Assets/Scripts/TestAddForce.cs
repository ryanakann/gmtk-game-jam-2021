using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAddForce : MonoBehaviour {

    // Update is called once per frame
    void Update() {
        GetComponent<Rigidbody2D>().AddForce(transform.up * 10f * Time.deltaTime);
    }
}
