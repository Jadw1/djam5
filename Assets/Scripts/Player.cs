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
[RequireComponent(typeof(AudioSource))]
public class Player : Entity<PlayerStats>
{
    public AudioClip onDeath;
    public AudioClip onHit;
    
    public GameObject body;
    private UserInterface ui;
    private AudioSource _audioSource;

    private void Start()
    {
        Init();
        ui = GetComponentInChildren<UserInterface>();
        ui.healthBar.SetMaxHealth(_stats.maxHealth);
        _audioSource = GetComponent<AudioSource>();

        _stats.OnMutationEvent += OnMutation;
    }

    private void OnMutation(Mutation mutation)
    {
        ui.mutationsList.AddMutation(mutation);
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
        _audioSource.PlayOneShot(onHit);
        ui.healthBar.SetHealth(_stats.health);
    }

    protected override void Die()
    {
        Debug.Log("You died!");
        _audioSource.PlayOneShot(onDeath);
        Destroy(body);
        ui.EndScreen();
    }
}