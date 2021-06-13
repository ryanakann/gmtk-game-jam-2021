using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DeliveryLocation : MonoBehaviour
{

    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

}
