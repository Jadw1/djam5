using System;
using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    private Transform player;
    public float smoothSpeed = 10f;
    public Vector3 offset;

    private void FixedUpdate() {
        if (!player) { 
            player = GameObject.FindWithTag("Player").transform;
            if(player)
                return;
        }

        Vector3 destination = player.position + offset;
        Vector3 smoothed = Vector3.Lerp(transform.position, destination, smoothSpeed * Time.deltaTime);
        transform.position = smoothed;
    }

}