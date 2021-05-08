using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 10f;
    [SerializeField] private float minimumDistanceToDealDamage = 0.1f;

    public TurretProjectile TurretOwner { get; set; }
    public float Damage { get; set; }

    protected Enemy enemyTarget;

    protected virtual void Update()
    {
        if (enemyTarget != null)
        {
            MoveProjectile();
            RotateProjectile();
        }
    }

    protected virtual void MoveProjectile()
    {
        transform.position =
            Vector2.MoveTowards(transform.position, enemyTarget.transform.position, moveSpeed * Time.deltaTime);

        float distanceToTarget = (enemyTarget.transform.position - transform.position).magnitude;

        if (distanceToTarget < minimumDistanceToDealDamage)
        {
            enemyTarget.EnemyHealth.DealDamage(Damage);
            
            TurretOwner.ResetTurretProjectile();

            ObjectPooler.ReturnToPool(gameObject);
        }
    }

    private void RotateProjectile()
    {
        Vector3 enemyPosition = enemyTarget.transform.position - transform.position;
        float angle = Vector3.SignedAngle(transform.up, enemyPosition, transform.forward);
        transform.Rotate(0f, 0f, angle);
    }

    public void SetEnemy(Enemy enemy)
    {
        enemyTarget = enemy;
    }

    public void ResetProjectile()
    {
        enemyTarget = null;
        
        transform.localRotation = Quaternion.identity;
    }
}
