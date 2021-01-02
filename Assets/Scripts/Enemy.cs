using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : Entity<EnemyStats>
{
    // public Color hitColor = Color.red;
    public float lerpFactor = 0.5f;
    
    // private Color _targetColor;
    // private Renderer _renderer;
    private NavMeshAgent _agent;

    private Transform _player;

    private void Start() 
    {    
        Init();
        _agent = GetComponent<NavMeshAgent>();
        // _renderer = GetComponent<Renderer>();
        // _targetColor = _renderer.material.color;

        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        GoToPlayer();
        // _renderer.material.color = Color.Lerp(_renderer.material.color, _targetColor, lerpFactor * Time.deltaTime);
    }

    private void GoToPlayer() {
        if (!_player) {
            return;
        }

        _agent.SetDestination(_player.position);
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
        // _renderer.material.color = hitColor;
    }
}
