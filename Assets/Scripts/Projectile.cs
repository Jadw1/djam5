using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage = 5.0f;
    public float speed = 5.0f;
    public ParticleSystem particleSystem;
    public Rigidbody rigidbody;

    private bool _hit;

    private void Start()
    {
        rigidbody.AddForce(transform.forward * speed, ForceMode.VelocityChange);
    }

    private void FixedUpdate()
    {
        if (_hit && particleSystem.isStopped)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        var player = other.collider.GetComponent<Player>();

        if (player != null)
        {
            player.TakeDamage(damage);
        }
        
        particleSystem.Play();
        _hit = true;
    }
}
