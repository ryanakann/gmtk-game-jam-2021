using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldModule : Module
{
    [SerializeField]
    float charge_used_per_second;

    [SerializeField]
    float charge_regened_per_second;
    float minimum_charge_percent_for_activation = 0.66f; // shields only turn back on once they regain this percentage of their max health

    bool is_active = false;

    [SerializeField]
    float max_charge;
    float current_charge;

    [SerializeField]
    float premature_activation_penalty;

    [SerializeField]
    float pop_disabling_radius;

    [SerializeField]
    float pop_disabling_duration;

    Collider2D shield_collider;

    protected override void Start()
    {
        base.Start();

        is_active = false;
        shield_collider.enabled = false;
        current_charge = max_charge;

        shield_collider = GetComponentInChildren<Collider2D>();

        if (shield_collider == null)
        {
            throw new System.NullReferenceException("ShieldModule needs a child Collider2D");
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
 
        if (is_active)
        {
            current_charge -= charge_used_per_second * Time.deltaTime;

            if (current_charge <= 0f)
            {
                Pop();
            }
        }
        else if (!is_disabled)
        {
            current_charge += charge_regened_per_second * Time.deltaTime;
        }
    }

    public override void OnButtonDown()
    {
        if (!is_active && current_charge >= (max_charge * minimum_charge_percent_for_activation))
            Activate();
        else if (is_active)
            Deactivate();
        else
            current_charge -= premature_activation_penalty;
    }

    void Activate()
    {
        is_active = true;
        shield_collider.enabled = true;
    }

    void Deactivate()
    {
        is_active = false;
        shield_collider.enabled = false;
    }

    void Pop()
    {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, pop_disabling_radius);

        foreach (Collider2D collider in colliders)
        {
            Module module = collider.GetComponent<Module>();

            if (module != null)
            {
                module.Disable(pop_disabling_duration);
            }
        }

        Deactivate();
    }

    public override void Disable(float seconds_disabled)
    {
        base.Disable(seconds_disabled);
        Deactivate();
    }

    public void ShieldDamage(float damage_amount)
    {
        current_charge -= damage_amount;

        if (current_charge < 0f)
        {
            current_charge = 0f;
            Pop();
        }
    }

}
