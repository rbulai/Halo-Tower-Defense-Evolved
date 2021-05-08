using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurretCard : MonoBehaviour
{
    public static Action<TurretSettings> OnPlaceTurret;
    
    [SerializeField] private Image turretImage;
    [SerializeField] private TextMeshProUGUI turretCost;
    
    public TurretSettings TurretLoaded { get; set; }

    public void SetUpTurretButton(TurretSettings turretSettings)
    {
        TurretLoaded = turretSettings;
        turretImage.sprite = turretSettings.TurretShopSprite;
        turretCost.text = turretSettings.TurretShopCost.ToString();
    }

    public void PlaceTurret()
    {
        if (CurrencySystem.Instance.TotalSupply >= TurretLoaded.TurretShopCost)
        {
            CurrencySystem.Instance.RemoveSupply(TurretLoaded.TurretShopCost);
            
            UIManager.Instance.CloseTurretShopPanel();
            
            OnPlaceTurret?.Invoke(TurretLoaded);
        }
    }
}
