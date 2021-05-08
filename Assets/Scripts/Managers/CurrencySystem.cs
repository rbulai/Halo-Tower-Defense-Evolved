using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencySystem : Singleton<CurrencySystem>
{
    [SerializeField] private int supplyTest;
    
    private string CURRENCY_SAVE_KEY = "MYGAME_CURRENCY";
    
    public int TotalSupply { get; set; }

    private void Start()
    {
        PlayerPrefs.DeleteKey(CURRENCY_SAVE_KEY);
        
        LoadSupply();
    }

    private void LoadSupply()
    {
        TotalSupply = PlayerPrefs.GetInt(CURRENCY_SAVE_KEY, supplyTest);
    }

    public void AddSupply(int amount)
    {
        TotalSupply += amount;
        
        PlayerPrefs.SetInt(CURRENCY_SAVE_KEY, TotalSupply);
        PlayerPrefs.Save();
    }

    public void RemoveSupply(int amount)
    {
        if (TotalSupply >= amount)
        {
            TotalSupply -= amount;
            
            PlayerPrefs.SetInt(CURRENCY_SAVE_KEY, TotalSupply);
            PlayerPrefs.Save();
        }
    }

    private void AddSupply(Enemy enemy)
    {
        AddSupply(1);
    }

    private void OnEnable()
    {
        EnemyHealth.OnEnemyKilled += AddSupply;
    }

    private void OnDisable()
    {
        EnemyHealth.OnEnemyKilled -= AddSupply;
    }
}
