using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineTurretProjectile : TurretProjectile
{
    [SerializeField] private bool isDualMachineTurret;
    [SerializeField] private float spreadRange;
    
    protected override void Update()
    {
        if (Time.time > nextAttackTime)
        {
            if (turret.CurrentEnemyTarget != null)
            {
                Vector3 directionToTarget = turret.CurrentEnemyTarget.transform.position - transform.position;
            
                FireProjectile(directionToTarget);
            }

            nextAttackTime = Time.time + delayBetweenAttacks;
        }
    }

    protected override void LoadProjectile() { }

    private void FireProjectile(Vector3 direction)
    {
        GameObject instance = pooler.GetInstanceFromPool();

        instance.transform.position = projectileSpawnPosition.position;

        MachineProjectile projectile = instance.GetComponent<MachineProjectile>();
        projectile.Direction = direction;
        projectile.Damage = Damage;

        if (isDualMachineTurret)
        {
            float randomSpread = Random.Range(-spreadRange, spreadRange);

            Vector3 spread = new Vector3(0f, 0f, randomSpread);
            
            Quaternion spreadValue = Quaternion.Euler(spread);

            Vector2 newDirection = spreadValue * direction;

            projectile.Direction = newDirection;
        }
        
        instance.SetActive(true);
    }
}
