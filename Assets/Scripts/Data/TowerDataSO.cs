using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerData", menuName = "TowerDataSO")]
public class TowerDataSO : ScriptableObject
{
    public ETowerType Type;
    public int towerCost;
    public float fireRate;
    public float towerRange;
}
