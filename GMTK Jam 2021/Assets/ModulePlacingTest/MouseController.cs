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
            print("oh boy we hit a something!");
            Module mod = hit.collider.GetComponentInParent<Module>();
            if (mod)
            {
                print("and that something was a module!");
                if (targetModule && mod != targetModule && !down)
                {
                    // clear targetModule
                    targetModule = null;
                }
                // clicking on a module should display information in the corner of the screen
                if (mod.isDetached)
                {
                    print("and that module was a detached module!");
                    if (down && changeState)
                    {
                        print("and boy howdy did we click it");
                        // play sound effect
                        // update ui
                        targetModule = mod.transform;
                    }
                }
            }
        }
    }
}
