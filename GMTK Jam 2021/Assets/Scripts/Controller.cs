using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {
    [SerializeField]
    bool player;

    HashSet<KeyCode> pressedKeys;
    HashSet<KeyCode> heldKeys;
    HashSet<KeyCode> releasedKeys;

    private void Start() {
        pressedKeys = new HashSet<KeyCode>();
        heldKeys = new HashSet<KeyCode>();
        releasedKeys = new HashSet<KeyCode>();
    }

    private void Update()
    {
        releasedKeys.Clear();
        pressedKeys.Clear();
    }

    public bool GetKey(KeyCode key) {
        if (player) {
            return Input.GetKey(key);
        }
        return heldKeys.Contains(key);
    }

    public bool GetKeyDown(KeyCode key)
    {
        if (player)
        {
            return Input.GetKeyDown(key);
        }
        return pressedKeys.Contains(key);
    }

    public bool GetKeyUp(KeyCode key)
    {
        if (player)
        {
            return Input.GetKeyUp(key);
        }
        return releasedKeys.Contains(key);
    }

    public void press(KeyCode key) {
        pressedKeys.Add(key);
        heldKeys.Add(key);
    }
    public void release(KeyCode key) {
        pressedKeys.Remove(key);
        heldKeys.Remove(key);
        releasedKeys.Add(key);
    }
}