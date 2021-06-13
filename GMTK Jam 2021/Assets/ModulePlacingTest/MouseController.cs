using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public static MouseController instance;

    [HideInInspector] public Transform targetModule;

    bool changeState, down;

    public LayerMask mask;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool newState = (Input.GetMouseButton(0));
        changeState = newState == down;
        down = newState;

        if (!down && changeState)
        {
            targetModule = null;
        }

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 1, mask);
        if (hit.collider != null)
        {
            Module mod = hit.collider.GetComponent<Module>();
            if (mod)
            {
                if (targetModule && mod != targetModule && !down)
                {
                    // clear targetModule
                    targetModule = null;
                }
                // clicking on a module should display information in the corner of the screen
                if (mod.isDetached)
                {
                    if (down && changeState)
                    {
                        // play sound effect
                        // update ui
                        targetModule = mod.transform;
                    }
                }
            }
        }
    }
}
