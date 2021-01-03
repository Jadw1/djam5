using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public GameObject player;
    private Vector3 _offset;

    private void LateUpdate () 
    {
        if (!player) {
            player = GameObject.FindWithTag("Player");
            if(player)
                return;
            else
                _offset = transform.position - player.transform.position;
        }
        
        transform.position = player.transform.position + _offset;
    }
}