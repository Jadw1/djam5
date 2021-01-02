using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : Entity<EnemyStats>
{
    private NavMeshAgent _agent;
    private Rigidbody _rigidbody;
    private ParticleSystem _particleSystem;

    private Transform _player;

    private void Start() 
    {
        Init();
        _agent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody>();
        _particleSystem = GetComponent<ParticleSystem>();

        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        GoToPlayer();
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
        _particleSystem.Play();
    }

    public void PushBack(Vector3 origin, float force)
    {
        // fifty is a magic number
        _rigidbody.AddExplosionForce(force * 50, origin, 0);
    }
}
