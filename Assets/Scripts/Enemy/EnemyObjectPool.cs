using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool
{
    public int poolSize;
    public GameObject prefab;
}

public class EnemyObjectPool : MonoBehaviour
{
    [SerializeField] private Queue<GameObject> enemyPoolQueue = new Queue<GameObject>();
    [SerializeField] private List<Pool> enemyPools;

    private int activeEnemyCount = 0;
    public int ActiveEnemyCount => activeEnemyCount;

    private int randomEnemyPoolIndex;
    private Transform enemyObjectPoolParent;

    void Awake()
    {
        ServiceLocator.Register(this);

        activeEnemyCount = 0;
        PopulateEnemyPool();
    }

    void PopulateEnemyPool()
    {
        for (int i = 0; i < enemyPools[randomEnemyPoolIndex].poolSize; i++)
        {
            RandomizeEnemy();
            var enemyInstance = Instantiate(enemyPools[randomEnemyPoolIndex].prefab);
            enemyInstance.SetActive(false);
            EnqueueEnemy(enemyInstance);
            enemyInstance.transform.parent = enemyObjectPoolParent;
        }
    }

    void RandomizeEnemy()
    {
        randomEnemyPoolIndex = Mathf.FloorToInt(Random.Range(0, enemyPools.Count));
    }

    public void EnableEnemyInPool(Transform enemyStartPoint)
    {
        GameObject activeEnemyInstance;

        if (enemyPoolQueue.Count > 0)
        {
            var dequeuedEnemy = DequeueEnemy();
            dequeuedEnemy.gameObject.SetActive(true);
            activeEnemyInstance = dequeuedEnemy.gameObject;
        }
        else
        {
            RandomizeEnemy();
            activeEnemyInstance = Instantiate(enemyPools[randomEnemyPoolIndex].prefab.gameObject);
        }

        activeEnemyCount++;
        SetPositionAndRotation(enemyStartPoint, activeEnemyInstance);
    }

    private void SetPositionAndRotation(Transform enemyStartPoint, GameObject activeEnemyInstance)
    {
        activeEnemyInstance.transform.position = enemyStartPoint.position;
        activeEnemyInstance.transform.rotation = enemyStartPoint.rotation;
        activeEnemyInstance.transform.parent = enemyObjectPoolParent;
    }

    public void EnqueueEnemy(GameObject enemyInstance)
    {
        enemyInstance.gameObject.SetActive(false);
        if (activeEnemyCount > 0)
            activeEnemyCount--;
        enemyPoolQueue.Enqueue(enemyInstance);
    }

    private GameObject DequeueEnemy()
    {
        return enemyPoolQueue.Dequeue();
    }
}