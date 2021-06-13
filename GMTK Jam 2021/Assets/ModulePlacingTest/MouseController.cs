using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{

    enum MouseState { Up, Released, Pressed, Down };

    public static MouseController instance;

    [HideInInspector] public GameObject targetModule;

    [HideInInspector] MouseState mouseState;

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
        if (Input.GetMouseButton(0))
            mouseState = (mouseState == MouseState.Up || mouseState == MouseState.Released) ? MouseState.Pressed : MouseState.Down;
        else
            mouseState = (mouseState == MouseState.Down || mouseState == MouseState.Pressed) ? MouseState.Released : MouseState.Up;

        if (mouseState == MouseState.Released)
        {
            // clear targetModule
        }

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 1, mask);
        if (hit.collider != null)
        {
            Module mod = hit.collider.GetComponent<Module>();
            if (mod)
            {
                // clicking on a module should display information in the corner of the screen
                // if floating, activate dragging mode
                if (mod.isDetached)
                {

                }
            }
            Debug.Log("Target Position: " + hit.collider.gameObject.transform.position);
        }
    }
}
