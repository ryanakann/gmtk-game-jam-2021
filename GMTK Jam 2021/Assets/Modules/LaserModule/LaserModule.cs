using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserModule : ModuleBehavior
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

    [SerializeField]
    ParticleSystem particles;

    public override void OnButtonDown()
    {
        particles.Play();
    }

    public override void OnButtonHeld()
    {
        current_heat += heatup_rate * Time.deltaTime;

        if (current_heat >= heat_cap)
            module.Explode(explode_power, explode_radius);
        else
            Shoot();
    }

    public override void OnButtonUp()
    {
        particles.Pause();
    }

    void Shoot()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up);

        if (hit.collider != null)
        {
            ShieldModule shield = hit.collider.GetComponent<ShieldModule>();
            Module module = hit.collider.GetComponent<Module>();
            if (shield != null)
            {
                shield.ShieldDamage(damage_per_second * Time.deltaTime);
            }
            else if (module != null)
            {
                module.Damage(damage_per_second * Time.deltaTime);
            }
        }
    }

}
