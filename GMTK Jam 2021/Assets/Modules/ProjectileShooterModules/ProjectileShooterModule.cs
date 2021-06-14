using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShooterModule : ModuleBehavior
{

    [SerializeField]
    float cooldown_between_shots;
    float cooldown_timer;

    [SerializeField]
    float premature_shot_penalty;

    [SerializeField]
    Transform shot_point;

    [SerializeField]
    GameObject projectile_prefab;

    protected override void Update()
    {
        base.Update();

        if (cooldown_timer >= 0f)
        {
            cooldown_timer -= Time.deltaTime;
        }
    }

    public override void OnButtonDown()
    {
        if (cooldown_timer >= 0)
        {
            cooldown_timer += premature_shot_penalty;
        }
        else
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Instantiate(projectile_prefab, shot_point.position, shot_point.rotation);
    }
}
