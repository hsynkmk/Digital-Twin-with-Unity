using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class DragAndDropMine : MonoBehaviour
{
    [SerializeField] private GameObject[] buildObjects; // Array of objects to build
    [SerializeField] private Transform resourceObject;

    private GameObject selectedObject; // The currently selected object
    private bool selected; // The preview object
    private RaycastHit hit; // The object that was hit by the ray
    private Ray ray; // The ray to cast from the camera to the mouse position
    private int selectedMineTypeIndex = -1; // Index of the selected mine type, -1 means no mine type is selected


    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            PlaceObject();
        }
    }

    // Place the selected object in the scene at the target position
    private void PlaceObject()
    {
        if (selected)
        {
            // The user has selected a mine type and should now click a conveyor
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Ground"))
            {
                GameObject newMine = Instantiate(selectedObject, hit.point, Quaternion.identity);
                newMine.transform.SetParent(resourceObject);
                Objects.availableResources.Enqueue(newMine.transform);
                selectedMineTypeIndex = -1; // Reset the selected mine type index
                selected = false; // Reset the selected flag
            }
        }
    }

    // Select an object from the build menu
    public void SelectObject(int index)
    {
        if (index >= 0 && index < buildObjects.Length)
        {

            // Set the selected mine type index and return
            selected = true;
            selectedMineTypeIndex = index;
            selectedObject = buildObjects[index];
            return;

        }
    }

}