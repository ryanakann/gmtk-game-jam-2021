using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {
    [SerializeField]
    bool player;

    HashSet<KeyCode> pressedKeys;

    private void Start() {
        pressedKeys = new HashSet<KeyCode>();
    }

    public bool GetKey(KeyCode key) {
        if (player) {
            return Input.GetKey(key);
        }
        return pressedKeys.Contains(key);
    }

    public void press(KeyCode key) {
        pressedKeys.Add(key);
    }
    public void release(KeyCode key) {
        pressedKeys.Remove(key);
    }
}