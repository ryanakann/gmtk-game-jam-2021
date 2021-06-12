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
                Deactivate();
            }
        }
        else
        {
            current_charge += charge_regened_per_second * Time.deltaTime;
        }
    }

    public override void OnButtonDown()
    {
        if (current_charge >= (max_charge * minimum_charge_percent_for_activation))
            Activate();
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

    public void ShieldDamage(float damage_amount)
    {
        current_charge -= damage_amount;

        if (current_charge < 0f)
            current_charge = 0f;
    }

}
