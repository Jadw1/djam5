using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Animator _animator;
    private Rigidbody _rigidBody;
    private Vector3 _movement;
    private Weapon _weapon;
    private static readonly int Speed = Animator.StringToHash("Speed");

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _weapon = GetComponent<Weapon>();
    }

    private void FixedUpdate()
    {
        _movement.x = Input.GetAxis("Horizontal");
        _movement.z = Input.GetAxis("Vertical");
        
        _animator.SetFloat(Speed, _movement.sqrMagnitude);

        _rigidBody.velocity = _movement.normalized * speed;
        _rigidBody.rotation = _weapon.transform.rotation;
    }

    public void AttackAnim()
    {
        _animator.SetTrigger("Attack");
    }
}