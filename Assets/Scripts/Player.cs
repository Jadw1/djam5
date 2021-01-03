using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

[RequireComponent(typeof(PlayerStats))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerRotation))]
public class Player : Entity<PlayerStats>
{
    public GameObject body;
    private UserInterface ui;

    private void Start()
    {
        Init();
        ui = GetComponentInChildren<UserInterface>();
        ui.healthBar.SetMaxHealth(_stats.maxHealth);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Alive())
        {
            if (Time.timeScale == 0)
                ResumeGame();
            else
                PauseGame();
        }
    }
    
    void PauseGame()
    {
        ui.PauseScreen();
    }

    public void ResumeGame()
    {
        ui.Resume();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        ui.healthBar.SetHealth(_stats.health);
    }

    protected override void Die()
    {
        Debug.Log("You died!");
        Destroy(body);
        ui.EndScreen();
    }
}