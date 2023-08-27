using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDropMine : MonoBehaviour
{
    [SerializeField] private GameObject[] minePrefabs; // Array of mine prefabs
    [SerializeField] private Transform resourceObject; // Parent transform for placed mines

    private GameObject selectedMinePrefab; // The currently selected mine prefab
    private RaycastHit hit; // The object that was hit by the ray
    private Ray ray; // The ray to cast from the camera to the mouse position

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            TryPlaceMine();
        }
    }

    // Attempt to place the selected mine prefab
    private void TryPlaceMine()
    {
        if (selectedMinePrefab != null)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Ground"))
            {
                PlaceMinePrefabAt(hit.point);
                ClearSelectedMine();
            }
        }
    }

    // Place the selected mine prefab at the given position
    private void PlaceMinePrefabAt(Vector3 position)
    {
        GameObject newMine = Instantiate(selectedMinePrefab, position, Quaternion.identity);
        newMine.transform.SetParent(resourceObject);
        ResourceManager.availableResources.Enqueue(newMine.transform); // Assuming this handles resource availability
    }

    // Select a mine from the build menu
    public void SelectMine(int index)
    {
        if (IsValidMineIndex(index))
        {
            selectedMinePrefab = minePrefabs[index];
        }
    }

    // Check if the mine index is valid
    private bool IsValidMineIndex(int index)
    {
        return index >= 0 && index < minePrefabs.Length;
    }

    // Clear the selected mine prefab and reset placement
    private void ClearSelectedMine()
    {
        selectedMinePrefab = null;
    }
}
