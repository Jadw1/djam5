using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Entity<T>: MonoBehaviour where T: Stats {
    public T _stats;

    protected void Init() {
        _stats = GetComponent<T>();
        _stats.health = _stats.maxHealth;
    }

    public virtual void TakeDamage(float damage)
    {
        _stats.health -= damage;
        if (_stats.health <= 0)
        {
            Die();
        }
    }
    
    void Die()
    {
        Debug.Log(name + " Died");
        Destroy(gameObject);
    }
    
}