using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulePlacementHandler : MonoBehaviour
{
    Transform portPoints, activatedPort, activeModule;
    bool validPort;

    void Awake()
    {
        portPoints = transform.FindDeepChild("PortPoints");
    }

    void Update()
    {
        CheckActivatedPort();   
    }

    private void CheckActivatedPort()
    {
        if (!activatedPort)
            return;

        validPort = Physics2D.OverlapCircle(activatedPort.position, 0.5f);
        
        if (validPort)
        {
            // show the good green symbol
        }
        else
        {
            // show the big bad red symbol
        }
    }

    private void OnMouseOver()
    {
        if (MouseController.instance.targetModule)
        {
            activeModule = MouseController.instance.targetModule;
            float minDistance = float.PositiveInfinity;
            Transform port = null;
            foreach (Transform t in portPoints)
            {
                float dist = Vector2.Distance(t.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
                if (dist < minDistance)
                {
                    minDistance = dist;
                    port = t;
                }
            }
            if (port && activatedPort && port != activatedPort)
            {
                activatedPort.gameObject.SetActive(false);
            }
            activatedPort = port;
            activatedPort?.gameObject?.SetActive(true);
        }
    }

    private void OnMouseExit()
    {
        activeModule = null;
        activatedPort?.gameObject?.SetActive(false);
    }

    private void OnMouseUp()
    {
        print("SCREEEEEEE " + activeModule);
        if (activeModule && validPort)
        {
            Module targetModule = activeModule.GetComponent<Module>();
            targetModule.SetParent(GetComponent<Module>(), activatedPort);
        }
    }
}
