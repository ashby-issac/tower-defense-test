using UnityEngine;

public class MouseClickDebugger : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log($"Hit Object: {hit.collider.gameObject.name}");
                Debug.Log($"Hit Position: {hit.point}");
            }
            else
            {
                Debug.Log("No object hit");
            }
        }
    }
}