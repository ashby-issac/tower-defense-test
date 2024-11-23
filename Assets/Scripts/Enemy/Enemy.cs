using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

interface IDamageable
{
    void TakeDamage(float damage);
}

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private float closeDist = 1f;
    [SerializeField] private float maxHealth = 50;
    [SerializeField] private float damage = 10; // damage for the player base
    [SerializeField] private float hitPoints = 4; // gold earned when enemy is defeated

    private float health = 50;

    private NavMeshAgent agent;
    private Transform houseTransform;

    private EnemyObjectPool enemyObjectPool;

    private void OnEnable()
    {
        health = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health < 1)
        {
            enemyObjectPool.EnqueueEnemy(gameObject);
            GameManager.Instance.OnEnemyDefeated(hitPoints);
        }
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        houseTransform = GameManager.Instance.HouseTransform;

        enemyObjectPool = ServiceLocator.Get<EnemyObjectPool>();

    }

    void Update()
    {
        if (GameManager.Instance.IsGameOver && agent.isActiveAndEnabled)
        {
            agent.ResetPath();
            return;
        }

        if (agent != null && agent.isActiveAndEnabled && gameObject.activeInHierarchy)
        {
            agent.SetDestination(houseTransform.position);
            if (agent.hasPath && agent.remainingDistance < closeDist)
            {
                GameManager.Instance.DamagePlayerBase(damage);
                enemyObjectPool.EnqueueEnemy(gameObject);
            }
        }
    }
}
