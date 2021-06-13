using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuxuryModule : Module
{
    [SerializeField]
    float initial_satisfaction_gain;

    [SerializeField]
    float satisfaction_gain_per_second;

    protected override void Update()
    {
        base.Update();

        foreach (Module child in children)
        {
            Deliverable deliverable = child.GetComponent<Deliverable>();
            SatisfyDeliverable(deliverable);
        }
    }

    void SatisfyDeliverable(Deliverable deliverable)
    {

        if (deliverable != null
            && (deliverable.DType == DeliveryType.Punko
            || deliverable.DType == DeliveryType.Corpo
            || deliverable.DType == DeliveryType.Crimo))
        {
            deliverable.IncreaseSatisfaction(satisfaction_gain_per_second * Time.deltaTime);
        }

    }

    public override void AddChild(Module child)
    {
        base.AddChild(child);
        Deliverable deliverable = child.gameObject.GetComponent<Deliverable>();

        if (deliverable != null
            && (deliverable.DType == DeliveryType.Punko
            || deliverable.DType == DeliveryType.Corpo
            || deliverable.DType == DeliveryType.Crimo))
        {
            deliverable.IncreaseSatisfaction(initial_satisfaction_gain);
        }
    }
}
