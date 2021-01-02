using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Game/Weapon")]
public class WeaponType : ScriptableObject
{
    [Range(0, 100)]
    public float range = 2.0f;
    
    [Range(0, 360)]
    public float angle = 45.0f;

    [Range(1f, 360f)]
    public float raycastDensity = 1.0f;
}
