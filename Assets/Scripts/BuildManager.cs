using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildManager : MonoBehaviour
{
    [SerializeField] private GameObject[] buildObjects; // Array of objects to build
    [SerializeField] private TextMeshProUGUI[] objectCountTexts; // UI text to display the number of machines left

    private GameObject selectedObject; // The currently selected object
    private GameObject previewObject; // The preview object
    private RaycastHit hit; // The object that was hit by the ray
    private Ray ray; // The ray to cast from the camera to the mouse position
    private bool isPreviewing; // Whether or not the preview object is active
    private int selectedObjectIndex; // Index of the currently selected object in the buildObjects array
    private int[] objectCounts; // Array to store the counts of available machines
    private int selectedMineTypeIndex = -1; // Index of the selected mine type, -1 means no mine type is selected

    private void Start()
    {
        objectCounts = new int[buildObjects.Length];
        for (int i = 0; i < objectCounts.Length; i++)
        {
            objectCounts[i] = 4; // Set the initial count to 4 for each machine type
            UpdateMachineCountUI(i);
        }
    }

    private void Update()
    {
        HandlePreview();

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            PlaceObject();
        }
    }

    // Handle the previewing of the selected object
    private void HandlePreview()
    {
        if (isPreviewing)
        {
            // Hide preview when hovering over a UI element
            if (EventSystem.current.IsPointerOverGameObject())
            {
                if (previewObject.activeSelf)
                {
                    previewObject.SetActive(false);
                    return;
                }
            }
            else if (!previewObject.activeSelf)
                previewObject.SetActive(true);

            // Right click to cancel previewing
            if (Input.GetMouseButtonDown(1))
            {
                DestroyPreviewObject();
                isPreviewing = false;
            }
            else
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Ground"))
                {
                    MovePreviewObject(hit.point);
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    RotatePreviewObject();
                }
            }
        }
    }

    // Move the preview object to the target position
    private void MovePreviewObject(Vector3 targetPosition)
    {
        previewObject.transform.position = targetPosition;
    }

    // Rotate the preview object by 90 degrees
    private void RotatePreviewObject()
    {
        previewObject.transform.Rotate(Vector3.up, 90f);
    }


    // Place the selected object in the scene at the target position
    private void PlaceObject()
    {
        if (selectedMineTypeIndex != -1)
        {
            // The user has selected a mine type and should now click a conveyor
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Conveyor"))
            {
                DecreaseObjectCount(selectedObjectIndex);
                InstantiateSelectedObject(hit.point);
                selectedMineTypeIndex = -1; // Reset the selected mine type index
            }
        }
        else if (previewObject != null)
        {
            // Decrement the count and update the UI text
            DecreaseObjectCount(selectedObjectIndex);

            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Ground"))
            {
                InstantiateSelectedObject(hit.point);
            }

            previewObject = null;
            isPreviewing = false;
        }
    }

    // Instantiate the selected object at the given position
    private void InstantiateSelectedObject(Vector3 position)
    {
        Instantiate(selectedObject, position, Quaternion.identity);
    }

    // Select an object from the build menu
    public void SelectObject(int index)
    {
        if (index >= 0 && index < buildObjects.Length)
        {
            if (objectCounts[index] > 0)
            {
                if (IsMineType(index))
                {previewObject=null; isPreviewing = false;
                    // Set the selected mine type index and return
                    selectedMineTypeIndex = index;
                    selectedObject = buildObjects[index];
                    selectedObjectIndex = index;
                    return;
                }
                selectedObjectIndex = index;
                selectedObject = buildObjects[index];
                DestroyPreviewObject();
                CreatePreviewObject();
                isPreviewing = true;
            }
            EventSystem.current.SetSelectedGameObject(null); // Cancel keyboard (pressing space etc.)
        }
    }

    private bool IsMineType(int index)
    {
        // Assuming mine types are placed at the start of the buildObjects array
        return index >= 4 && index < 8;
    }

    private void UpdateMachineCountUI(int index)
    {
        if (index >= 0 && index < objectCountTexts.Length)
        {
            objectCountTexts[index].text = objectCounts[index].ToString();
        }
    }

    // Destroy the preview object if it exists
    private void DestroyPreviewObject()
    {
        if (previewObject != null)
        {
            Destroy(previewObject);
        }
    }

    // Create a preview object for the selected object
    private void CreatePreviewObject()
    {
        previewObject = Instantiate(selectedObject);
    }

    // Decrease the count of an object at the given index
    private void DecreaseObjectCount(int index)
    {
        if (objectCounts[index] > 0)
        {
            objectCounts[index]--;
            UpdateMachineCountUI(index);
        }
    }
}