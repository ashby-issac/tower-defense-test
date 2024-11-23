using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.UI;

public enum ETowerType
{
    Bullet, Arrow
}

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject towerSelectionPanel;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI waveSignalText;

    private CurrencySystem currencySystem;

    private void Awake()
    {
        ServiceLocator.Register(this);
    }

    private void Start()
    {
        currencySystem = ServiceLocator.Get<CurrencySystem>();

        GameManager.Instance.OnPlayerHealthUpdated += UpdateHealth;
        currencySystem.OnCoinsUpdated += UpdateCoins;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnPlayerHealthUpdated -= UpdateHealth;
        currencySystem.OnCoinsUpdated -= UpdateCoins;
    }

    public void ShowTowersInfo(bool active)
    {
        towerSelectionPanel.SetActive(active);
    }

    public void OnSelectedTower(ETowerType type)
    {
        switch (type)
        {
            case ETowerType.Bullet:
                GameManager.Instance.OnTowerSelected(ETowerType.Bullet);
                break;
            case ETowerType.Arrow:
                GameManager.Instance.OnTowerSelected(ETowerType.Arrow);
                break;
        }
    }

    private void UpdateCoins(int coins)
    {
        coinsText.text = $"Coins: {coins}";
    }

    private void UpdateHealth(int health)
    {
        healthText.text = $"Health: {health}";
    }

    public void ShowWaveSignalText(bool state, int waveNumber)
    {
        waveSignalText.enabled = state;
        waveSignalText.text = $"Incoming wave: {waveNumber}";
    }
}
