using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepulserModule : ModuleBehavior
{

    [SerializeField]
    float repulse_force;

    [SerializeField]
    float repulse_range;

    [SerializeField]
    float repulse_angle;

    public override void OnButtonDown()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, repulse_range);

        foreach (Collider2D collider in colliders)
        {
            // FIXME(Simon): make sure that the repulser is not repulsing modules on the same
            //               ship that it is attached to

            Vector3 direction = (collider.transform.position - transform.position).normalized;

            float angle = Vector3.Angle(transform.forward, direction);

            if (Mathf.Abs(angle) > repulse_angle)
                continue;

            Rigidbody2D rb = collider.attachedRigidbody;

            if (rb != null)
            {
                rb.AddExplosionForce(repulse_force, transform.position, repulse_range);
            }
        }
    }

}
