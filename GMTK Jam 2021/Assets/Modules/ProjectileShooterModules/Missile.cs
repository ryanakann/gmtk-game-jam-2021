using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Projectile
{
    [SerializeField]
    float speed;

    [SerializeField]
    float explosion_radius;

    [SerializeField]
    float explosion_power;

    public Rigidbody2D rigidBody;
    
    [SerializeField]
    float angle_changing_speed;

    [SerializeField]
    float movement_speed;

    [SerializeField]
    float search_radius;

    Transform target;

    private void Start()
    {
        FindTarget();
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            Vector2 direction = (Vector2)target.position - rb.position;
            direction.Normalize();
            float rotateAmount = Vector3.Cross(direction, transform.up).z;
            rigidBody.angularVelocity = -angle_changing_speed * rotateAmount;
        }
        else
        {
            FindTarget();
        }
        rigidBody.velocity = transform.up * movement_speed;
    }

    void FindTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, search_radius);

        float closest_distance = Mathf.Infinity;
        Module closest_module = null;

        foreach (Collider2D collider in colliders)
        {
            Module module = collider.GetComponent<Module>();

            if (module != null && module != owner)
            {
                float distance = (transform.position - module.transform.position).magnitude;

                if (distance < closest_distance)
                {
                    closest_module = module;
                }
            }
        }

        if (closest_module != null)
        {
            target = closest_module.transform;
        }
    }

    protected override void Hit(GameObject obj_hit)
    {
        Explode();
    }

    protected void Explode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosion_radius);

        foreach (Collider2D collider in colliders)
        {
            Rigidbody2D rb = collider.attachedRigidbody;

            if (rb != null)
            {
                rb.AddExplosionForce(explosion_power, transform.position, explosion_radius);
            }

            Module module = collider.GetComponent<Module>();
            if (module != null)
            {
                Vector2 diff = collider.transform.position - transform.position;
                float distance = diff.magnitude;
                module.Damage(explosion_power / distance);
            }
        }
    }
}