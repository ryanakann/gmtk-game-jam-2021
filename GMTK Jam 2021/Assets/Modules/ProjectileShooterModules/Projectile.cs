using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public abstract class Projectile : MonoBehaviour
{
    protected Module owner;
    protected Rigidbody2D rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetOwner(Module module)
    {
        owner = module;
    }

    abstract protected void Hit(GameObject obj_hit);

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Module module = collision.gameObject.GetComponent<Module>();

        if (module != owner)
        {
            Hit(collision.gameObject);
        }
    }
}
