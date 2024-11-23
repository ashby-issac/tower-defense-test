using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveData
{
    public int numberOfWavesInCurrentWave;
    public int totalNumberOfEnemies;
    public int enemySpawnDelay;
    public int wavesDelay;
    public int incomingWaveDelay;
}

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField] private List<WaveData> waveDatas;
    [SerializeField] private Transform[] spawnPoints;

    [HideInInspector] public bool HasFinishedSpawning = false;

    private EnemyObjectPool enemyObjectPool;

    private void Awake()
    {
        HasFinishedSpawning = false;
        ServiceLocator.Register(this);
    }

    void Start()
    {
        enemyObjectPool = ServiceLocator.Get<EnemyObjectPool>();
        StartEnemyMovement();
    }

    public void StartEnemyMovement()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        int wavesControllerIndex = 0;
        int currentWaveNumber = 0;

        yield return new WaitForSeconds(2f);
        GameManager.Instance.DisplayWaveSignalText(true, wavesControllerIndex + 1);
        yield return new WaitForSeconds(2f);
        GameManager.Instance.DisplayWaveSignalText(false, wavesControllerIndex + 1);

        while (true)
        {
            if (wavesControllerIndex > waveDatas.Count - 1) 
            { 
                break; 
            }

            if (currentWaveNumber < waveDatas[wavesControllerIndex].numberOfWavesInCurrentWave)
            {
                for (int j = 0; j < waveDatas[wavesControllerIndex].totalNumberOfEnemies; j++)
                {
                    enemyObjectPool.EnableEnemyInPool(spawnPoints[Random.Range(0, spawnPoints.Length)]);
                    yield return new WaitForSeconds(waveDatas[wavesControllerIndex].enemySpawnDelay);
                }
                yield return new WaitForSeconds(waveDatas[wavesControllerIndex].wavesDelay);
                currentWaveNumber++;
            }
            else if (currentWaveNumber == waveDatas[wavesControllerIndex].numberOfWavesInCurrentWave)
            {
                wavesControllerIndex++;
                currentWaveNumber = 0;
                if (wavesControllerIndex != waveDatas.Count)
                {
                    GameManager.Instance.DisplayWaveSignalText(true, wavesControllerIndex + 1);
                    yield return new WaitForSeconds(waveDatas[wavesControllerIndex].incomingWaveDelay);
                    GameManager.Instance.DisplayWaveSignalText(false, wavesControllerIndex + 1);
                }
            }
        }

        HasFinishedSpawning = true;
    }
}
