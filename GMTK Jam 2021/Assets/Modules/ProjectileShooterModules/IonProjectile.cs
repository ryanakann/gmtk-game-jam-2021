using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class IonProjectile : Projectile
{

    [SerializeField]
    float speed;

    [SerializeField]
    float disable_time;

    protected override void Awake()
    {
        base.Awake();
        rb.velocity = transform.forward * speed;
    }

    protected override void Hit(GameObject obj_hit)
    {
        ShieldModule shield = obj_hit.GetComponent<ShieldModule>();
        Module module = obj_hit.GetComponent<Module>();
        if (shield != null)
        {
            shield.ShieldDamage(disable_time);
        }
        else if (module != null)
        {
            module.Disable(disable_time);
        }

        Destroy(gameObject);
    }

}
