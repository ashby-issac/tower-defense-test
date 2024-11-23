using UnityEngine;

public class TowerBase : MonoBehaviour
{
    [SerializeField] private bool isOccupied;

    public bool IsOccupied => isOccupied;

    private void OnMouseDown()
    {
        if (!isOccupied && !GameManager.Instance.IsGameOver)
        {
            GameManager.Instance.OnTowerBaseSelected(this);
        }
    }

    public void PlaceTower(GameObject towerPrefab)
    {
        var instance = Instantiate(towerPrefab, transform.position, Quaternion.identity);
        var tower = instance.GetComponent<Tower>();

        GameManager.Instance.AddNewTowerInstance(new TowersList { towerType = tower.TowerDataSO.Type, tower = tower });

        isOccupied = true;
    }
}
