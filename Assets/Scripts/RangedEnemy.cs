using UnityEngine;

public class RangedEnemy : Enemy
{
    public GameObject projectile;
    
    private void Start()
    {
        base.Start();
    }

    protected override void OnAttacking()
    {
        var forward = (_player.transform.position - transform.position).normalized;
        Instantiate(projectile, transform.position + forward * 0.35f + Vector3.up, Quaternion.LookRotation(forward));
    }
}
