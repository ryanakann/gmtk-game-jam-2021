using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserModule : Module
{

    [SerializeField]
    float damage_per_second;

    [SerializeField]
    float heat_cap;

    [SerializeField]
    float heatup_rate;
    float current_heat;

    [SerializeField]
    float explode_power;

    [SerializeField]
    float explode_radius;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Activate()
    {
        current_heat += heatup_rate * Time.deltaTime;

        if (current_heat >= heat_cap)
            Explode(explode_power, explode_radius);
        else
            Shoot();
    }

    void Shoot()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up);

        if (hit.collider != null)
        {
            Module module = hit.collider.GetComponent<Module>();
            if (module != null)
            {
                module.Damage(damage_per_second * Time.deltaTime);
            }
        }
    }

}
