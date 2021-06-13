using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deliverable : Module
{

    [SerializeField]
    float max_payout;

    [SerializeField]
    float starting_satisfaction;

    [SerializeField]
    float satisfaction_max;

    [SerializeField]
    float satisfaction_reduction_per_second;

    [SerializeField]
    float jostle_velocity_threshold;

    [SerializeField]
    float jostle_satisfaction_coefficient;

    float satisfaction;

    Transform destination;

    private void Awake()
    {
        satisfaction = starting_satisfaction;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        satisfaction -= satisfaction_reduction_per_second * Time.deltaTime;
        
    }

    public void IncreaseSatisfaction(float satisfaction_amount)
    {
        satisfaction += satisfaction_amount;
    }

    public void DecreaseSatisfaction(float satisfaction_amount)
    {
        satisfaction -= satisfaction_amount;
    }

    void GetDelivered()
    {
        float payout = max_payout * (satisfaction / satisfaction_max);
        GameManager.instance.Pay(payout);
    }

    public void Jostle(float impulse)
    {
        if (impulse >= jostle_velocity_threshold)
        {
            satisfaction -= impulse * jostle_satisfaction_coefficient;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DeliveryLocation location = collision.GetComponent<DeliveryLocation>();
        if (location != null)
        {
            GetDelivered();
        }
    }
}
