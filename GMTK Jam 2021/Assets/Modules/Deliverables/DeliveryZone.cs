using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryZone : MonoBehaviour
{

    [SerializeField]
    GameObject[] delivery_prefabs;

    [SerializeField]
    Transform[] delivery_spots;
    List<Transform> empty_delivery_spots;

    [SerializeField]
    DeliveryLocation[] delivery_locations;

    int max_active_exchanges = 3;
    int active_exchanges = 0;

    [SerializeField]
    float min_cooldown;

    [SerializeField]
    float max_cooldown;

    float cooldown;

    // Start is called before the first frame update
    void Start()
    {
        SpawnDeliverable();
    }

    // Update is called once per frame
    void Update()
    {
        if (empty_delivery_spots.Count <= 0 && active_exchanges < max_active_exchanges && cooldown >= 0f)
        {
            cooldown -= Time.deltaTime;

            if (cooldown <= 0f)
                SpawnDeliverable();
        }
    }

    public DeliveryLocation GetDeliveryLocation()
    {
        return delivery_locations[Random.Range(0,delivery_locations.Length)];
    }

    void EmptyDeliverySpot(Transform delivery_spot)
    {
        empty_delivery_spots.Add(delivery_spot);
    }

    void ResolveExchange()
    {
        if (cooldown <= 0f)
            cooldown = Random.Range(min_cooldown, max_cooldown);
        active_exchanges--;
    }

    void SpawnDeliverable()
    {
 
        Transform delivery_spot = delivery_spots[Random.Range(0, delivery_spots.Length)];

        DeliveryZone zone_to_deliver_to = DeliveryManager.instance.GetZoneForDelivery(this);
        if (zone_to_deliver_to == null)
        {
            cooldown = Random.Range(min_cooldown, max_cooldown);
            return;
        }

        zone_to_deliver_to.EstablishExchange();

        GameObject delivery_prefab = delivery_prefabs[Random.Range(0, delivery_prefabs.Length)];
        GameObject new_delivery_obj = Instantiate(delivery_prefab, delivery_spot.position, Quaternion.identity);

        empty_delivery_spots.Remove(delivery_spot);

        Deliverable new_delivery = new_delivery_obj.GetComponent<Deliverable>();
        new_delivery.SetSpawnPoint(delivery_spot);
        new_delivery.delivery_resolution += ResolveExchange;
        new_delivery.delivery_resolution += zone_to_deliver_to.ResolveExchange;
        new_delivery.empty_spot_event += EmptyDeliverySpot;

        new_delivery.SetDeliveryLocation(zone_to_deliver_to.GetDeliveryLocation());

        active_exchanges++;
    }

    public void EstablishExchange()
    {
        active_exchanges++;
    }

    public bool CanAcceptExchange()
    {
        return active_exchanges < max_active_exchanges;
    }

}
