using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBase : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 100f;
    
    private float health;

    private void Start()
    {
        Invoke(nameof(InitializeHealth), 1f);
    }

    private void InitializeHealth()
    {
        health = maxHealth;
        GameManager.Instance.OnPlayerHealthUpdated?.Invoke((int)health);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        GameManager.Instance.OnPlayerHealthUpdated?.Invoke((int)health);
        if (health < 1)
        {
            GameManager.Instance.OnGameOver();
        }
    }
}
