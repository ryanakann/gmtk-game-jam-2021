using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class IonProjectile : MonoBehaviour
{

    [SerializeField]
    float speed;

    [SerializeField]
    float disable_time;

    private void Awake()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.forward * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ShieldModule shield = collision.gameObject.GetComponent<ShieldModule>();
        Module module = collision.gameObject.GetComponent<Module>();
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
