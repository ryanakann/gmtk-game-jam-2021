using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void DeliveryEvent();
public delegate void EmptySpot(Transform delivery_spot);

public enum DeliveryType { Punko, Corpo, Crimo, Valuables, Illicits, Volatiles };

public class Deliverable : Module
{

    public DeliveryEvent delivery_resolution;
    public EmptySpot empty_spot_event;

    [SerializeField]
    DeliveryType type;
    public DeliveryType DType { get { return type; } }

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

    [SerializeField]
    float timeout_time;

    float satisfaction;

    Transform destination;
    Transform spawn_point;

    private void Awake()
    {
        satisfaction = starting_satisfaction;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (spawn_point)
        {
            timeout_time -= Time.deltaTime;
            if (timeout_time <= 0f)
            {
                Die();
            }
        }
        else
        {
            satisfaction -= satisfaction_reduction_per_second * Time.deltaTime;
        }
    }

    public void SetSpawnPoint(Transform in_spawn_point)
    {
        spawn_point = in_spawn_point;
    }

    public void SetDeliveryLocation(DeliveryLocation delivery_location)
    {
        destination = delivery_location.transform;
    }

    public void IncreaseSatisfaction(float satisfaction_amount)
    {
        satisfaction += satisfaction_amount;
        if (satisfaction > satisfaction_max)
            satisfaction = satisfaction_max;
    }

    public void DecreaseSatisfaction(float satisfaction_amount)
    {
        satisfaction -= satisfaction_amount;
        if (satisfaction < 0f)
            satisfaction = 0f;
    }

    void GetDelivered()
    {
        float percent_satisfied = (satisfaction / satisfaction_max);
        float payout = max_payout * percent_satisfied;
        GameManager.instance.Pay(payout);
        GameManager.instance.Rate(percent_satisfied);

        mainModule.GetComponent<MainModule>().RemoveDeliverable(this);

        delivery_resolution.Invoke();
    }

    public override void SetParent(Module parent, Transform pivot)
    {
        base.SetParent(parent, pivot);
        mainModule.GetComponent<MainModule>().AddDeliverable(this);

        empty_spot_event.Invoke(spawn_point);
        spawn_point = null;
    }

    public override void Die()
    {
        if (spawn_point)
            empty_spot_event.Invoke(spawn_point);
        delivery_resolution.Invoke();
        base.Die();
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
