using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction {
    FORWARD = (1 << 0),
    RIGHT = (1 << 1),
    BACKWARD = (1 << 2),
    LEFT = (1 << 3)
}

public enum ElementType {
    ENTER = (1 << 0),
    CROSSING = (1 << 1),
    EXIT = (1 << 2),
    DEAD_END = (1 << 3)
}

[Serializable]
public struct CrossingEntity {
    public Transform crossing;
    public Direction direction;

    [Space(5)] 
    public bool avoidDirection;
    public Direction toAvoid;
}

public class RoomElement : MonoBehaviour {
    public ElementType type;
    [Space(10)]
    
    public Transform[] enemySpawnPoints = new Transform[0];
    public List<CrossingEntity> crossings = new List<CrossingEntity>(0);
    
    [Space(10)] 
    [Header("For enter")] 
    public Transform playerSpawnPoint;

    private WorldCollider[] colliders;
    
    public void RegisterElement(Action collisionFunc) {
        colliders = GetComponentsInChildren<WorldCollider>();
        foreach (var collider in colliders) {
            collider.RegisterCollider(collisionFunc);
        }
    }
}
