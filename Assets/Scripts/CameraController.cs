using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public GameObject player;
    private Vector3 _offset;

    private void Start () 
    {
        _offset = transform.position - player.transform.position;
    }
    
    private void LateUpdate () 
    {
        transform.position = player.transform.position + _offset;
    }
}