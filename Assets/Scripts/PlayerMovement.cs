using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float thresholdSpeed = 0.2f;
    
    private Player _player;
    private Vector3 _movement;
    private Animator _animator;
    private Rigidbody _rigidBody;
    private Weapon _weapon;
    private static readonly int Speed = Animator.StringToHash("Speed");

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody>();
        _weapon = GetComponent<Weapon>();
        _player = GetComponent<Player>();
    }

    private void FixedUpdate()
    {
        _movement.x = Input.GetAxis("Horizontal");
        _movement.z = Input.GetAxis("Vertical");
        
        _animator.SetFloat(Speed, _movement.sqrMagnitude);

        float speed = _player._stats.speed;
        if (Mathf.Abs(speed) <= 0.0f) {
            speed = thresholdSpeed;
        }

        _rigidBody.velocity = _movement.normalized * speed;
        _rigidBody.rotation = _weapon.transform.rotation;
    }

    public void AttackAnim()
    {
        _animator.SetTrigger("Attack");
    }
}