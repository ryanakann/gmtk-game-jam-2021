using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserModule : ModuleBehavior
{

    [SerializeField]
    Transform shoot_point;

    [SerializeField]
    float damage_per_second;

    [SerializeField]
    float heat_cap;

    [SerializeField]
    float heatup_rate;
    float current_heat;

    [SerializeField]
    float cool_rate;

    [SerializeField]
    float explode_power;

    [SerializeField]
    float explode_radius;

    [SerializeField]
    ParticleSystem particles;

    [SerializeField]
    SpriteRenderer diamond;

    bool shooting = false;

    protected override void Update()
    {
        base.Update();

        if (shooting)
        {
            current_heat += heatup_rate * Time.deltaTime;

            float heat_percent = (current_heat / heat_cap) * 1.2f;
            diamond.color = new Color(1f, 1f - heat_percent, 1f - heat_percent);

            Shoot();
        }
        else if (current_heat >= 0f)
        {
            current_heat -= cool_rate * Time.deltaTime;
        }

        if (current_heat >= heat_cap)
        {
            print("we are over heat capacity");
            module.Explode(explode_power, explode_radius);
        }

    }

    public override void OnButtonDown()
    {
        particles.Play();
    }

    public override void OnButtonHeld()
    {
        shooting = true;
    }

    public override void OnButtonUp()
    {
        particles.Stop();
    }

    void Shoot()
    {
        RaycastHit2D hit = Physics2D.Raycast(shoot_point.position, shoot_point.forward);

        if (hit.collider != null)
        {
            ShieldModule shield = hit.collider.GetComponentInParent<ShieldModule>();
            Module module = hit.collider.GetComponentInParent<Module>();
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
