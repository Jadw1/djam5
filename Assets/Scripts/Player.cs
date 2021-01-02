using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    private PlayerMovement _pm;
    private PlayerRotation _pr;
    

    private void Start()
    {
        _pm = gameObject.AddComponent<PlayerMovement>();
        _pr = gameObject.AddComponent<PlayerRotation>();
    }
}
