using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [Header("Panels")]
    [SerializeField] private GameObject turretShopPanel;
    [SerializeField] private GameObject nodeUIPanel;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI upgradeText;
    [SerializeField] private TextMeshProUGUI sellText;
    [SerializeField] private TextMeshProUGUI turretLevelText;
    [SerializeField] private TextMeshProUGUI totalSupplyText;
    [SerializeField] private TextMeshProUGUI totalLivesText;
    [SerializeField] private TextMeshProUGUI currentWaveText;
    
    private Node currentNodeSelected;

    private void Update()
    {
        totalSupplyText.text = CurrencySystem.Instance.TotalSupply.ToString();

        totalLivesText.text = LevelManager.Instance.TotalLives.ToString();

        currentWaveText.text = $"Wave {LevelManager.Instance.CurrentWave}";
    }

    public void CloseTurretShopPanel()
    {
        turretShopPanel.SetActive(false);
    }

    public void CloseNodeUIPanel()
    {
        currentNodeSelected.CloseAttackRangeSprite();
        
        nodeUIPanel.SetActive(false);
    }

    public void UpgradeTurret()
    {
        currentNodeSelected.Turret.TurretUpgrade.UpgradeTurret();
        
        UpdateUpgradeText();
        UpdateTurretLevel();
        UpdateSellValue();
    }

    public void SellTurret()
    {
        currentNodeSelected.SellTurret();
        currentNodeSelected = null;
        
        nodeUIPanel.SetActive(false);
    }

    private void ShowNodeUI()
    {
        nodeUIPanel.SetActive(true);
        
        UpdateUpgradeText();
        UpdateTurretLevel();
        UpdateSellValue();
    }

    private void UpdateUpgradeText()
    {
        upgradeText.text = currentNodeSelected.Turret.TurretUpgrade.UpgradeCost.ToString();
    }

    private void UpdateTurretLevel()
    {
        turretLevelText.text = $"Level {currentNodeSelected.Turret.TurretUpgrade.Level}";
    }

    private void UpdateSellValue()
    {
        int sellAmount = currentNodeSelected.Turret.TurretUpgrade.GetSellValue();

        sellText.text = sellAmount.ToString();
    }

    private void NodeSelected(Node nodeSelected)
    {
        currentNodeSelected = nodeSelected;

        if (currentNodeSelected.IsEmpty())
        {
            turretShopPanel.SetActive(true);
        }

        else
        {
            ShowNodeUI();
        }
    }
    
    private void OnEnable()
    {
        Node.OnNodeSelected += NodeSelected;
    }

    private void OnDisable()
    {
        Node.OnNodeSelected -= NodeSelected;
    }
}
