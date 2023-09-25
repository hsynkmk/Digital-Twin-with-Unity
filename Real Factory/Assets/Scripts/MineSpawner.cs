using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDropMine : MonoBehaviour
{
    [SerializeField] private GameObject[] minePrefabs; // Array of mine prefabs
    [SerializeField] private Transform resourceObject; // Parent transform for placed mines
    [SerializeField] private Transform spawnPosition; // The position to place the mine prefab

    private GameObject selectedMinePrefab; // The currently selected mine prefab

    private void Update()
    {

        TryPlaceMine();

    }

    // Attempt to place the selected mine prefab
    private void TryPlaceMine()
    {
        if (selectedMinePrefab != null)
        {
            PlaceMinePrefabAt();
            ClearSelectedMine();
        }
    }

    // Place the selected mine prefab at the given position
    private void PlaceMinePrefabAt()
    {
        ResourceManager.availableResources.Enqueue(selectedMinePrefab.transform);
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
