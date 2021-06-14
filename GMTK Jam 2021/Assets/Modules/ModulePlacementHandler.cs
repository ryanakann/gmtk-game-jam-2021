using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulePlacementHandler : MonoBehaviour
{
    [HideInInspector] public Transform portPoints, activatedPort;
    bool validPort;

    void Awake()
    {
        portPoints = transform.FindDeepChild("PortPoints");
    }

    private void Update()
    {
        CheckActivatedPort();
    }

    public void CheckActivatedPort()
    {
        if (!activatedPort)
            return;

        validPort = !Physics2D.OverlapCircle(activatedPort.position, 0.25f);

        if (validPort)
        {
            // show the good green symbol
        }
        else
        {
            // show the big bad red symbol
        }
    }

    public void MouseOver()
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
        if (port && activatedPort && port != activatedPort)
        {
            activatedPort.gameObject.SetActive(false);
        }
        activatedPort = port;
        if (activatedPort)
            activatedPort.gameObject.SetActive(true);
    }

    public void ClearPort()
    {
        if (activatedPort)
        {
            activatedPort.gameObject.SetActive(false);
            activatedPort = null;
        }            
    }

    private void OnMouseExit()
    {
        ClearPort();    
    }

    public void AddModule(Module module)
    {
        if (validPort)
            module.SetParent(GetComponent<Module>(), activatedPort);
    }
}
