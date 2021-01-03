using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void LateUpdate()
    {
        var rotation = _camera.transform.rotation;
        var worldPosition = transform.position + rotation * Vector3.forward;
        var worldUp = rotation * Vector3.up;
        
        transform.LookAt(worldPosition, worldUp);
    }
}
