using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDropMine : MonoBehaviour
{
    [SerializeField] private GameObject[] minePrefabs; // Array of mine prefabs
    [SerializeField] private Transform resourceObject; // Parent transform for placed mines

    private GameObject selectedMinePrefab; // The currently selected mine prefab
    private bool isPlacingMine; // Flag to indicate if a mine is currently being placed
    private RaycastHit hit; // The object that was hit by the ray
    private Ray ray; // The ray to cast from the camera to the mouse position

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            PlaceMine();
        }
    }

    // Place the selected mine in the scene at the target position
    private void PlaceMine()
    {
        if (isPlacingMine)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Ground"))
            {
                InstantiateAndSetParent(selectedMinePrefab, hit.point);
                ResetPlacement();
            }
        }
    }

    // Instantiate the given prefab and set its parent
    private void InstantiateAndSetParent(GameObject prefab, Vector3 position)
    {
        GameObject newMine = Instantiate(prefab, position, Quaternion.identity);
        newMine.transform.SetParent(resourceObject);
        ResourceManager.availableResources.Enqueue(newMine.transform); // Assuming this handles resource availability
    }

    // Select a mine from the build menu
    public void SelectMine(int index)
    {
        if (index >= 0 && index < minePrefabs.Length)
        {
            selectedMinePrefab = minePrefabs[index];
            isPlacingMine = true;
        }
    }

    // Reset placement variables
    private void ResetPlacement()
    {
        isPlacingMine = false;
        selectedMinePrefab = null;
    }
}
