using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DamageOnTouch : MonoBehaviour
{
    public float damageAmount = 1f;
    public float damageCooldown = 0.5f;

    private float lastDamageTime = -Mathf.Infinity;

    private void OnCollisionStay2D(Collision2D collision)
    {
        TryDamage(collision.collider);
    }

    private void TryDamage(Collider2D collider)
    {
        if (!collider.CompareTag("Player")) return;

        if (Time.time < lastDamageTime + damageCooldown)
            return;
        
        var health = collider.GetComponent<Health>();
        
        health?.TakeDamage(damageAmount);
        lastDamageTime = Time.time;
    }
}
