using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretProjectile : MonoBehaviour
{
    [SerializeField] protected Transform projectileSpawnPosition;
    [SerializeField] protected float delayBetweenAttacks = 2f;
    [SerializeField] protected float damage = 2f;

    public float Damage { get; set; }
    public float DelayPerShot { get; set; }

    protected float nextAttackTime;

    protected ObjectPooler pooler;

    protected Turret turret;

    protected Projectile currentProjectileLoaded;

    private void Start()
    {
        turret = GetComponent<Turret>();
        pooler = GetComponent<ObjectPooler>();
        
        Damage = damage;
        DelayPerShot = delayBetweenAttacks;
        
        LoadProjectile();
    }

    protected virtual void Update()
    {
        if (IsTurretEmpty())
        {
            LoadProjectile();
        }

        if (Time.time > nextAttackTime)
        {
            if (turret.CurrentEnemyTarget != null && currentProjectileLoaded != null &&
                turret.CurrentEnemyTarget.EnemyHealth.CurrentHealth > 0f)
            {
                currentProjectileLoaded.transform.parent = null;
                currentProjectileLoaded.SetEnemy(turret.CurrentEnemyTarget);
            }

            nextAttackTime = Time.time + DelayPerShot;
        }
    }

    protected virtual void LoadProjectile()
    {
        GameObject newInstance = pooler.GetInstanceFromPool();

        newInstance.transform.localPosition = projectileSpawnPosition.position;
        
        newInstance.transform.SetParent(projectileSpawnPosition);

        currentProjectileLoaded = newInstance.GetComponent<Projectile>();

        currentProjectileLoaded.TurretOwner = this;
        currentProjectileLoaded.ResetProjectile();
        currentProjectileLoaded.Damage = Damage;
        
        newInstance.SetActive(true);
    }

    private bool IsTurretEmpty()
    {
        return currentProjectileLoaded == null;
    }

    public void ResetTurretProjectile()
    {
        currentProjectileLoaded = null;
    }
}
