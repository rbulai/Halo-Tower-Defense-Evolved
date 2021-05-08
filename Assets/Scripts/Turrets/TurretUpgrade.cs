using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretUpgrade : MonoBehaviour
{
    [SerializeField] private int upgradeInitialCost;
    [SerializeField] private int upgradeCostIncremental;
    [SerializeField] private float damageIncremental;
    [SerializeField] private float delayReduction;

    [Header("Sell")] [Range(0, 1)] [SerializeField]
    private float sellPercentage;

    public float SellPercentage { get; set; }
    public int UpgradeCost { get; set; }
    public int Level { get; set; }

    private TurretProjectile turretProjectile;

    private void Start()
    {
        turretProjectile = GetComponent<TurretProjectile>();
        UpgradeCost = upgradeInitialCost;
        SellPercentage = sellPercentage;

        Level = 1;
    }

    public void UpgradeTurret()
    {
        if (CurrencySystem.Instance.TotalSupply >= UpgradeCost)
        {
            turretProjectile.Damage += damageIncremental;
            turretProjectile.DelayPerShot -= delayReduction;

            UpdateUpgrade();
        }
    }

    public int GetSellValue()
    {
        int sellValue = Mathf.RoundToInt(UpgradeCost * SellPercentage);

        return sellValue;
    }

    private void UpdateUpgrade()
    {
        CurrencySystem.Instance.RemoveSupply(UpgradeCost);

        UpgradeCost += upgradeCostIncremental;

        Level++;
    }
}
