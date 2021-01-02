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

    [Range(0.0f, 60.0f)]
    public float cooldownDuration = 0.5f;

    [Range(0.0f, 100.0f)]
    public float damage = 10.0f;
    
    public GameObject displayModel;
}
