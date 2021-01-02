using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private Camera _camera;
    private Plane _plane;
    private float _cooldown;

    public WeaponType weaponType;
    private void Start()
    {
        _camera = Camera.main;
        _plane = new Plane(Vector3.up, Vector3.zero);
        _cooldown = Time.time;
    }

    [CanBeNull]
    private Enemy TryAttackDirection(Vector3 startPos, Vector3 forward, float angle)
    {
        var direction = Quaternion.AngleAxis(angle, Vector3.up) * forward;

        if (Physics.Raycast(new Ray(startPos, direction), out var hit, direction.magnitude))
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
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        
        if (_plane.Raycast(ray, out var distance))
        {
            var mousePointInWorld = ray.GetPoint(distance);
            var direction = mousePointInWorld - transform.position;

            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
        
        var halfAngle = weaponType.angle / 2.0f;
        var raysCount = Mathf.FloorToInt(halfAngle / weaponType.raycastDensity);
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
            enemy.DoDamage(weaponType.damage);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _cooldown <= Time.time)
        {
            _cooldown = Time.time + weaponType.cooldownDuration;
            Attack();
        }
    }
}
