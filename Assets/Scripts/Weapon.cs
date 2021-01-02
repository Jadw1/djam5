using UnityEngine;

public class Weapon : MonoBehaviour
{
    private Camera _camera;
    private Plane _plane;

    public WeaponType weaponType;

    private void Start()
    {
        _camera = Camera.main;
        _plane = new Plane(Vector3.up, Vector3.zero);
    }

    private void Update()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        
        if (_plane.Raycast(ray, out var distance))
        {
            var mousePointInWorld = ray.GetPoint(distance);
            var direction = mousePointInWorld - transform.position;

            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }

        var forward = transform.forward * weaponType.range;

        var halfAngle = weaponType.angle / 2.0f;
        var raysCount = Mathf.FloorToInt(halfAngle / weaponType.raycastDensity);
        var stepAngle = halfAngle / raysCount;

        var position = transform.position;
        
        Debug.DrawLine(position, position + forward);
        
        for (var i = 1; i <= raysCount; i++)
        {
            var angle = stepAngle * i;
            
            var left = Quaternion.AngleAxis(angle, Vector3.up) * forward;
            var right = Quaternion.AngleAxis(-angle, Vector3.up) * forward;
            
            Debug.DrawLine(position, position + left);
            Debug.DrawLine(position, position + right);
        }
    }
}
