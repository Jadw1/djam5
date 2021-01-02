using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerStats))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerRotation))]
public class Player : Entity<PlayerStats> {

    private void Start() {
        Init();
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 3)
        {
            var enemy = collision.gameObject.GetComponent<Enemy>();
            TakeDamage(enemy._stats.damage);
        }
    }

    protected override void Die()
    {
        Debug.Log("You died!");
        SceneManager.LoadScene("DevArcana");
    }
}
