using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private TowerDataSO towerData;
    [SerializeField] private Transform towerWeapon;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject projectilePrefab;

    private List<GameObject> enemiesInRange = new List<GameObject>();
    private GameObject closeRangeTarget = null;
    private bool canShoot = true;
    private float shootTimer = 0f;

    public TowerDataSO TowerDataSO => towerData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            enemiesInRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            enemiesInRange.Remove(other.gameObject);
        }
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameOver) return;

        FindTarget();
        RefreshEnemiesInRange();
        Shoot();
        ShootDelay();
    }

    private void RefreshEnemiesInRange()
    {
        enemiesInRange.RemoveAll(enemy => !enemy.activeInHierarchy);
    }

    private void FindTarget()
    {
        foreach (var enemy in enemiesInRange)
        {
            if (enemy != null && enemy.activeInHierarchy)
            {
                closeRangeTarget = enemy;
                break;
            }
            else
            {
                closeRangeTarget = null;
            }
        }


        if (closeRangeTarget != null)
        {
            towerWeapon.LookAt(closeRangeTarget.transform);
        }
    }

    private void Shoot()
    {
        if (closeRangeTarget != null && canShoot &&
            Vector3.Distance(closeRangeTarget.transform.position, transform.position) <= towerData.towerRange)
        {
            var instance = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);
            var projectileMovement = instance.GetComponent<Projectile>();
            projectileMovement.MoveProjectile(spawnPoint.forward);
            canShoot = false;
        }
    }

    private void ShootDelay()
    {
        if (!canShoot && shootTimer < towerData.fireRate)
        {
            shootTimer += Time.deltaTime;
        }
        else
        {
            shootTimer = 0;
            canShoot = true;
        }
    }
}
