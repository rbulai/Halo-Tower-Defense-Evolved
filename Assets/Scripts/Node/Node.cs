using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public static Action<Node> OnNodeSelected;
    public static Action OnTurretSold;

    [SerializeField] private GameObject attackRangeSprite;
    
    public Turret Turret { get; set; }

    private float rangeSize;
    private Vector3 rangeOriginalSize;

    private void Start()
    {
        rangeSize = attackRangeSprite.GetComponent<SpriteRenderer>().bounds.size.y;
        rangeOriginalSize = attackRangeSprite.transform.localScale;
    }

    public void SetTurret(Turret turret)
    {
        Turret = turret;
    }

    public bool IsEmpty()
    {
        return Turret == null;
    }

    public void CloseAttackRangeSprite()
    {
        attackRangeSprite.SetActive(false);
    }

    public void SelectTurret()
    {
        OnNodeSelected?.Invoke(this);

        if (!IsEmpty())
        {
            ShowTurretInfo();
        }
    }

    public void SellTurret()
    {
        if (!IsEmpty())
        {
            CurrencySystem.Instance.AddSupply(Turret.TurretUpgrade.GetSellValue());
            
            Destroy(Turret.gameObject);
            
            Turret = null;
            
            attackRangeSprite.SetActive(false);
            
            OnTurretSold?.Invoke();
        }
    }

    private void ShowTurretInfo()
    {
        attackRangeSprite.SetActive(true);
        attackRangeSprite.transform.localScale = rangeOriginalSize * Turret.AttackRange / (rangeSize / 2);
    }
}
