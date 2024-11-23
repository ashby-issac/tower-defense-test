using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencySystem : MonoBehaviour
{
    [SerializeField] private int startingCoins = 30;

    private int currentCoins = 30;

    public event Action<int> OnCoinsUpdated;

    public int CurrentCoins => currentCoins;

    void Awake()
    {
        ServiceLocator.Register(this);

        InitializeCoins(startingCoins);
    }

    public void InitializeCoins(int startingAmount)
    {
        currentCoins = startingAmount;
        OnCoinsUpdated?.Invoke(currentCoins);
    }

    public bool CanSpendCoins(int amount)
    {
        if (currentCoins >= amount)
        {
            currentCoins -= amount;
            OnCoinsUpdated?.Invoke(currentCoins);
            return true;
        }

        return false;
    }

    public void AddCoins(int amount)
    {
        currentCoins += amount;
        OnCoinsUpdated?.Invoke(currentCoins);
    }
}
