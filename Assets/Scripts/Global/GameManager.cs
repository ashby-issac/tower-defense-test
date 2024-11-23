using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class TowersList
{
    public ETowerType towerType;
    public Tower tower;
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private TowersList[] availableTowers;

    private Transform houseTransform;
    private PlayerBase playerBase;
    private TowerBase selectedBase;
    private UIManager uiManager;
    private CurrencySystem currencySystem;

    [HideInInspector] public bool IsGameOver = false;
    [HideInInspector] public EnemyObjectPool EnemyObjectPool;
    [HideInInspector] public EnemySpawnManager EnemySpawnManager;
    [HideInInspector] public List<TowersList> activeTowers = new List<TowersList>();

    public Action<int> OnPlayerHealthUpdated;

    public Transform HouseTransform => houseTransform;

    public static GameManager Instance;

    public void AddNewTowerInstance(TowersList towerInfo)
    {
        activeTowers.Add(towerInfo);
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(Instance);
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        IsGameOver = false;
        uiManager = ServiceLocator.Get<UIManager>();
        currencySystem = ServiceLocator.Get<CurrencySystem>();
        EnemyObjectPool = ServiceLocator.Get<EnemyObjectPool>();
        EnemySpawnManager = ServiceLocator.Get<EnemySpawnManager>();

        houseTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerBase = houseTransform.GetComponent<PlayerBase>();
    }

    public void OnTowerBaseSelected(TowerBase towerBase)
    {
        selectedBase = towerBase;
        uiManager.ShowTowersInfo(true);
    }

    public void OnTowerSelected(ETowerType towerType)
    {
        if (selectedBase != null && !selectedBase.IsOccupied)
        {
            var selectedTower = availableTowers.FirstOrDefault(tower => tower.towerType == towerType);
            if (currencySystem.CanSpendCoins(selectedTower.tower.TowerDataSO.towerCost))
            {
                selectedBase.PlaceTower(selectedTower.tower.gameObject);
                uiManager.ShowTowersInfo(false);
            }
            else
            {
                uiManager.ShowTowersInfo(true);
            }
        }
    }

    public void DamagePlayerBase(float damage)
    {
        playerBase.TakeDamage(damage);
    }

    public void OnEnemyDefeated(float amount)
    {
        currencySystem.AddCoins((int)amount);
        if (EnemySpawnManager.HasFinishedSpawning && EnemyObjectPool.ActiveEnemyCount < 1)
        {
            OnGameOver();
        }
    }

    public void OnGameOver()
    {
        IsGameOver = true;
        ServiceLocator.ClearServices();
        Invoke(nameof(ReloadLevel), 3f);
    }

    private void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Invoke(nameof(Init), 1f);
    }

    public void DisplayWaveSignalText(bool state, int waveNumber)
    {
        uiManager.ShowWaveSignalText(state, waveNumber);
    }
}
