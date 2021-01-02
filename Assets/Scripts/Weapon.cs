using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform hand;
    public LineRenderer line;

    private bool _underCooldown;
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

    public void SetWeaponType(WeaponType type)
    {
        if (_weaponPrefab != null)
        {
            Destroy(_weaponPrefab);
        }

        _weaponPrefab = Instantiate(weaponType.displayModel, hand);
        _animator.SetFloat(AttackSpeed, 1.0f / type.cooldownDuration);

        weaponType = type;
    }

    private Enemy TryAttackDirection(Vector3 startPos, Vector3 direction, float angle)
    {
        if (Physics.Raycast(new Ray(startPos, direction), out var hit, direction.magnitude, _layerMask))
        {
            var enemy = hit.transform.GetComponent<Enemy>();

            if (enemy != null)
            {
                Debug.DrawLine(startPos, hit.point, Color.red, weaponType.cooldownDuration);
                return enemy;
            }
        }

        Debug.DrawLine(startPos, startPos + direction, Color.white, weaponType.cooldownDuration);
        return null;
    }

    private void Attack()
    {
        var position = transform.position;
        
        var halfAngle = weaponType.angle / 2.0f;
        var raysCount = Mathf.FloorToInt(halfAngle / weaponType.angleBetweenRaycasts);
        var stepAngle = halfAngle / raysCount;

        var enemiesHit = new HashSet<Enemy>();

        var points = new List<Vector3>();


        for (var i = -raysCount; i <= raysCount; i++)
        {
            var angle = stepAngle * i;
            var direction = Quaternion.AngleAxis(angle, Vector3.up) * transform.forward * weaponType.range;

            points.Add(position + direction + Vector3.up * 1.0f);
            var enemy = TryAttackDirection(position, direction, angle);
            if (enemy != null)
            {
                enemiesHit.Add(enemy);
            }
        }

        line.positionCount = points.Count;
        line.SetPositions(points.ToArray());

        // TODO: Possible optimization:
        // First get a hashset of transforms hit
        // then iterate and filter out those with Enemy component
        // Drawback: loss of debug lines indicating hit
        Debug.Log(enemiesHit.Count);
        foreach (var enemy in enemiesHit)
        {
            enemy.PushBack(position, weaponType.knockback);
            enemy.TakeDamage(weaponType.damage);
        }
    }

    private void Update()
    {
        if (_underCooldown && _cooldown <= Time.time)
        {
            line.positionCount = 0;
            line.SetPositions(new Vector3[] { });
            _underCooldown = false;
        }

        if (!_underCooldown && Input.GetMouseButtonDown(0))
        {
            _cooldown = Time.time + weaponType.cooldownDuration;
            _underCooldown = true;
            Attack();
            _playerMovement.AttackAnim();
        }
    }
}