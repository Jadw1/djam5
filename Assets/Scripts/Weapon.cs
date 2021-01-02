using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform hand;
    
    private float _cooldown;
    private int _layerMask;
    private PlayerMovement _playerMovement;
    private Animator _animator;
    private GameObject _weaponPrefab;

    public WeaponType weaponType;
    private static readonly int AttackSpeed = Animator.StringToHash("AttackSpeed");

    private void Start()
    {
        _layerMask = LayerMask.GetMask("Enemies", "World");
        _cooldown = Time.time;
        _playerMovement = gameObject.GetComponent<PlayerMovement>();
        _animator = GetComponent<Animator>();

        SetWeaponType(weaponType);
    }

    private void SetWeaponType(WeaponType type)
    {
        if (_weaponPrefab != null)
        {
            Destroy(_weaponPrefab);
        }
        
        _weaponPrefab = Instantiate(weaponType.displayModel, hand);
        _animator.SetFloat(AttackSpeed, 1.0f / type.cooldownDuration);

        weaponType = type;
    }

    private Enemy TryAttackDirection(Vector3 startPos, Vector3 forward, float angle)
    {
        var direction = Quaternion.AngleAxis(angle, Vector3.up) * forward;

        if (Physics.Raycast(new Ray(startPos, direction), out var hit, direction.magnitude, _layerMask))
        {
            var enemy = hit.transform.GetComponent<Enemy>();

            if (enemy != null)
            {
                Debug.DrawLine(startPos, hit.point, Color.red, weaponType.cooldownDuration);
                return enemy;
            }
            
            Debug.DrawLine(startPos, hit.point, Color.white, weaponType.cooldownDuration);
            return null;
        }

        Debug.DrawLine(startPos, startPos + direction, Color.white, weaponType.cooldownDuration);
        return null;
    }

    private void Attack()
    {
        var halfAngle = weaponType.angle / 2.0f;
        var raysCount = Mathf.FloorToInt(halfAngle / weaponType.angleBetweenRaycasts);
        var stepAngle = halfAngle / raysCount;

        var enemiesHit = new HashSet<Enemy>();

        for (var i = -raysCount; i <= raysCount; i++)
        {
            var angle = stepAngle * i;
            var enemy = TryAttackDirection(transform.position, transform.forward * weaponType.range, angle);
            if (enemy != null)
            {
                enemiesHit.Add(enemy);
            }
        }

        // TODO: Possible optimization:
        // First get a hashset of transforms hit
        // then iterate and filter out those with Enemy component
        // Drawback: loss of debug lines indicating hit
        foreach (var enemy in enemiesHit)
        {
            enemy.TakeDamage(weaponType.damage);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _cooldown <= Time.time)
        {
            _cooldown = Time.time + weaponType.cooldownDuration;
            Attack();
            _playerMovement.AttackAnim();
        }
    }
}
