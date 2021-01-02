using UnityEngine;

public class Weapon : MonoBehaviour
{
    private Camera _camera;
    private Plane _plane;

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
    }
}
