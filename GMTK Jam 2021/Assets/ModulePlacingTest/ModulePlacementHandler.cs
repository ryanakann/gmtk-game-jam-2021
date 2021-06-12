using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulePlacementHandler : MonoBehaviour
{
    Transform portPoints, activatedPort;
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
        validPort = Physics2D.OverlapCircle(activatedPort.position, 1f);
        
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
            if (port && port != activatedPort)
            {
                activatedPort.gameObject.SetActive(false);
                activatedPort = port;
            }
            activatedPort = port;
            activatedPort?.gameObject?.SetActive(true);
        }
    }

    private void OnMouseExit()
    {
        activatedPort?.gameObject?.SetActive(false);
    }

    private void OnMouseUp()
    {
        if (MouseController.instance.targetModule && validPort)
        {
            // place targetModule
        }
    }
}
