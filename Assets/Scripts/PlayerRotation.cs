using System;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    private Camera _camera;
    
    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Math.Abs(Time.timeScale - 1) < 0.1)
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);

            var plane = new Plane(Vector3.up, transform.position);

            if (plane.Raycast(ray, out var distance))
            {
                var mousePointInWorld = ray.GetPoint(distance);
                var direction = mousePointInWorld - transform.position;

                transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            }
        }
    }
}
