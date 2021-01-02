using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Entity : MonoBehaviour
{
    public float health;
    public float maxHealth = 100;
    public float speed = 5;
    public float size = 1;
    public float strength = 1;
    public float dexterity = 1;

    private void Start()
    {
        health = maxHealth;
    }

    void Update()
    {
        
    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }
    
    void Die()
    {
        Debug.Log("Died");
        Destroy(gameObject);
    }
    
}
