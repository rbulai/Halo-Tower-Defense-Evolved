using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankTurretProjectile : TurretProjectile
{
    protected override void Update()
    {
        if (Time.time > nextAttackTime)
        {
            if (turret.CurrentEnemyTarget != null && turret.CurrentEnemyTarget.EnemyHealth.CurrentHealth > 0)
            {
                FireProjectile(turret.CurrentEnemyTarget);
            }

            nextAttackTime = Time.time + delayBetweenAttacks;
        }
    }

    protected override void LoadProjectile() { }

    private void FireProjectile(Enemy enemy)
    {
        GameObject instance = pooler.GetInstanceFromPool();
        instance.transform.position = projectileSpawnPosition.position;

        Projectile projectile = instance.GetComponent<Projectile>();
        currentProjectileLoaded = projectile;
        currentProjectileLoaded.TurretOwner = this;
        currentProjectileLoaded.ResetProjectile();
        currentProjectileLoaded.SetEnemy(enemy);
        currentProjectileLoaded.Damage = Damage;
        
        instance.SetActive(true);
    }
}
