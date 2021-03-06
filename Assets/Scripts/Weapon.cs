using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Weapon : MonoBehaviour
{
    public Transform hand;
    public GameObject trail;

    public AudioClip[] swishes;

    private bool _underCooldown;
    private float _cooldown;
    private int _layerMask;
    private PlayerMovement _playerMovement;
    private Animator _animator;
    private GameObject _weaponPrefab;
    private AudioSource _audioSource;

    public WeaponType weaponType;
    private static readonly int AttackSpeed = Animator.StringToHash("AttackSpeed");

    private void Start()
    {
        _layerMask = LayerMask.GetMask("Enemies", "World");
        _cooldown = Time.time;
        _playerMovement = gameObject.GetComponent<PlayerMovement>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        SetWeaponType(weaponType);
    }

    public void SetWeaponType(WeaponType type)
    {
        if (_weaponPrefab != null)
        {
            Destroy(_weaponPrefab);
        }

        weaponType = type;
        
        _weaponPrefab = Instantiate(weaponType.displayModel, hand);
        _animator.SetFloat(AttackSpeed, 1.0f / type.cooldownDuration);
    }

    private void PlaySwish()
    {
        var swishIndex = Random.Range(0, swishes.Length);
        _audioSource.PlayOneShot(swishes[swishIndex]);
    }

    private void Attack()
    {
        PlaySwish();
        var position = transform.position;
        var points = new List<Vector3>();
        foreach (var result in Physics.OverlapSphere(position, weaponType.range, _layerMask))
        {
            var enemy = result.GetComponent<Enemy>();

            if (enemy != null)
            {
                var enemyPos = enemy.transform.position;
                var angle = Vector3.Angle(enemyPos - position, transform.forward);

                if (Mathf.Abs(angle) <= weaponType.angle / 2.0f)
                {
                    points.Add(enemyPos + Vector3.up * 0.5f);
                    enemy.PushBack(position, weaponType.knockback);
                    enemy.TakeDamage(weaponType.damage);
                }
            }
        }

        StartTrail(position, transform.forward);
    }
    
    private void StartTrail(Vector3 origin, Vector3 direction)
    {
        var count = 12;
        var points = new List<Vector3>();

        for (var i = -count; i < count; i++)
        {
            var point = Quaternion.AngleAxis((weaponType.angle / (2 * count)) * i, Vector3.up) * direction * (weaponType.range - 0.5f);
            points.Add(point);
        }
        
        var attackTrail = Instantiate(trail, origin + points[0] + Vector3.up, Quaternion.identity).GetComponent<AttackTrail>();
        attackTrail.StartTrail(weaponType.cooldownDuration, points, transform);
    }

    private void Update()
    {
        if (_underCooldown && _cooldown <= Time.time)
        {
            // line.positionCount = 0;
            // line.SetPositions(new Vector3[] { });
            _underCooldown = false;
        }

        if (!_underCooldown && Input.GetMouseButtonDown(0) && Math.Abs(Time.timeScale - 1) < 0.1)
        {
            _cooldown = Time.time + weaponType.cooldownDuration;
            _underCooldown = true;
            Attack();
            _playerMovement.AttackAnim();
        }
    }
}