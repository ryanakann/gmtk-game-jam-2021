using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager instance;

    DeliveryZone[] delivery_zones;

    public DeliveryZone GetZoneForDelivery(DeliveryZone delivering_zone)
    {

        delivery_zones.Shuffle();

        foreach (var zone in delivery_zones)
        {
            if (zone == delivering_zone)
                continue;
            else if (zone.CanAcceptExchange())
                return zone;
        }
        return null;
    }

}
