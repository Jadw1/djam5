using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerStats))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerRotation))]
public class Player : Entity<PlayerStats> {

    public HealthBar healthBar;
    
    private void Start() {
        Init();
        healthBar.SetMaxHealth(_stats.maxHealth);
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 3)
        {
            var enemy = collision.gameObject.GetComponent<Enemy>();
            TakeDamage(enemy._stats.damage);
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        
        healthBar.SetHealth(_stats.health);
    }
    
    protected override void Die()
    {
        Debug.Log("You died!");
        SceneManager.LoadScene("DevArcana");
    }
}
