using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulePlacementHandler : MonoBehaviour
{

    Transform activatedSector;
    bool validSector;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckActivatedSector();   
    }

    private void CheckActivatedSector()
    {
        // when activating sector
        // check collision in zone (have a hidden hex trigger thingy)
        // update highlight based on the hidden hex trigger thingy
    }

    private void OnMouseOver()
    {
        if (MouseController.instance.targetModule)
        {
            float minDistance = float.PositiveInfinity;
            Transform sector = null;
            foreach (Transform t in transform)
            {
                float dist = Vector2.Distance(t.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
                if (dist < minDistance)
                {
                    minDistance = dist;
                    sector = t;
                }
            }
            if (sector && sector != activatedSector)
            {
                activatedSector.gameObject.SetActive(false);
                activatedSector = sector;
            }
            activatedSector = sector;
            if (activatedSector)
            {
                activatedSector.gameObject.SetActive(true);
            }
        }
    }

    private void OnMouseExit()
    {
        if (activatedSector)
        {
            activatedSector.gameObject.SetActive(false);
        }
    }

    private void OnMouseUp()
    {
        if (MouseController.instance.targetModule && validSector)
        {
            // place targetModule
        }
    }
}
