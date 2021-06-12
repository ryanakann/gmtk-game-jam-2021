using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NanobotsModule : Module
{
    [SerializeField]
    float healing_per_second;

    [SerializeField]
    float healing_radius;

    [SerializeField]
    float healing_duration;
    float healing_timer = 0f;

    [SerializeField]
    float healing_cooldown;
    float cooldown_timer = 0f;

    bool is_active = false;

    protected override void Update()
    {
        if (is_active)
        {
            healing_timer -= Time.deltaTime;
            HealInRadius();

            if (healing_timer <= 0f)
                is_active = false;
        }
        else if (cooldown_timer >= 0f)
        {
            cooldown_timer -= Time.deltaTime;
        }
    }

    public override void OnButtonDown()
    {
        if (!is_active && cooldown_timer <= 0f)
        {
            healing_timer = healing_duration;
            is_active = true;
        }
    }

    void HealInRadius()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, healing_radius);

        foreach (Collider2D collider in colliders)
        {
            Module module = collider.GetComponent<Module>();

            if (module != null)
            {
                module.Heal(healing_per_second * Time.deltaTime);
            }
        }
    }

}
