using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction {
    FORWARD = (1 << 0),
    LEFT = (1 << 1),
    RIGHT = (1 << 2),
    BACKWARD = (1 << 3)
}

public enum ElementType {
    ENTER,
    CROSSING,
    EXIT,
    DEAD_END
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
    
    [SerializeField]
    private List<CrossingEntity> crossings = new List<CrossingEntity>(0);
}
