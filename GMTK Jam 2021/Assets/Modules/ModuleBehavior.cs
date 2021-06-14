using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleBehavior : MonoBehaviour
{
    HashSet<KeyCode> buttons;

    [SerializeField]
    protected bool active;

    [SerializeField]
    protected MainModule mainModule;
    Controller controller;

    [SerializeField]
    protected Module module;

    protected virtual void Awake()
    {
        if (!module)
            module = GetComponent<Module>();

        if (mainModule)
            controller = mainModule.GetComponent<Controller>();

        buttons = new HashSet<KeyCode>();
    }

    protected virtual void Update()
    {
        if (!active || !mainModule)
            return;

        bool buttonDown = false;
        bool buttonHeld = false;
        bool buttonUp = false;

        foreach (KeyCode button in buttons)
        {
            if (controller.GetKeyDown(button))
            {
                buttonDown = true;
            }            
            if (controller.GetKey(button))
            {
                buttonHeld = true;
            }
            if (controller.GetKeyUp(button))
            {
                buttonUp = true;
            }
        }
        if (buttonUp)
        {
            OnButtonUp();
        }
        if (buttonDown)
        {
            OnButtonDown();
        }
        if (buttonHeld)
        {
            OnButtonHeld();
        }
    }

    public virtual void SetMainModule(MainModule m)
    {
        mainModule = m;
        controller = mainModule.GetComponent<Controller>();
    }

    public virtual void Activate()
    {
        active = true;
    }

    public virtual void Deactivate()
    {
        active = false;
    }

    public void AssignButton(KeyCode key)
    {
        buttons.Add(key);
    }

    public bool UnassignButton(KeyCode key)
    {
        return buttons.Remove(key);
    }

    public virtual void Die()
    {
        //no thoughts head empty
    }

    public virtual void OnButtonDown()
    {
        //no thoughts head empty
    }

    public virtual void OnButtonHeld()
    {
        //no thoughts head empty
    }

    public virtual void OnButtonUp()
    {
        //no thoughts head empty
    }

}
