using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WorldCollider : MonoBehaviour {
    private Action collisionFunc;
    public void RegisterCollider(Action registerCollision) {
        collisionFunc = registerCollision;
    }
    
    private void OnCollisionStay(Collision other) {
        if (transform.parent != other.transform.parent) {
            collisionFunc();
            //Debug.Log($"{transform.parent.name}:{transform.name}  _____  {other.transform.parent.name}:{other.transform.name}");
        }
    }
}
