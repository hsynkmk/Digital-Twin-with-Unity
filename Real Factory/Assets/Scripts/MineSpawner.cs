using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

    /// <summary>
    ///     Spawn manager for mining objects
    ///     When the mine buttons clicked, the selected mining object's transform is added to the queue
    ///     The Instantiate process is managed in the RobotManager script
    /// </summary>

public class MineSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] minePrefabs; // Array of mine prefabs
    [SerializeField] private TextMeshProUGUI infoText;
    private static int cooperCount;
    private static int ironCount;
    private static int siliconCount;

    private GameObject selectedMinePrefab; // The currently selected mine prefab

    private void Update()
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

        infoText.text += $"{selectedMinePrefab.tag}\n";
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

    public void UpdateInfoText()
    {
        if (selectedMinePrefab.CompareTag("Cooper Mine"))
            cooperCount++;
        else if (selectedMinePrefab.CompareTag("Iron Mine"))
            ironCount++;
        else if (selectedMinePrefab.CompareTag("Silicon"))
            siliconCount++;
    }
}