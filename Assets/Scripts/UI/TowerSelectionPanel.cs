using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerSelectionPanel : MonoBehaviour
{
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private TowerDataSO[] towerDatasSO;

    private List<Button> towerButtons = new List<Button>();

    UIManager uiManager;

    private void Start()
    {
        uiManager = ServiceLocator.Get<UIManager>();

        PopulateButtons();
    }

    private void PopulateButtons()
    {
        foreach (var towerData in towerDatasSO)
        {
            var btnGOInstance = Instantiate(buttonPrefab);

            btnGOInstance.transform.SetParent(buttonContainer);

            btnGOInstance.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

            var button = btnGOInstance.GetComponent<Button>();
            towerButtons.Add(button);
            button.onClick.AddListener(() => uiManager.OnSelectedTower(towerData.Type));
            btnGOInstance.GetComponentInChildren<TextMeshProUGUI>().text = $"{towerData.Type.ToString()} - {towerData.towerCost}"; 
        }
    }
}
