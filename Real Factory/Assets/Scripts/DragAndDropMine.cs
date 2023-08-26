using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDropMine : MonoBehaviour
{
    [SerializeField] private GameObject[] buildObjects; // Array of objects to build
    [SerializeField] private Transform resourceObject;

    private GameObject selectedObject; // The currently selected object
    private GameObject previewObject; // The preview object
    private RaycastHit hit; // The object that was hit by the ray
    private Ray ray; // The ray to cast from the camera to the mouse position
    private int selectedObjectIndex; // Index of the currently selected object in the buildObjects array
    private int selectedMineTypeIndex = -1; // Index of the selected mine type, -1 means no mine type is selected


    private void Update()
    {

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            PlaceObject();
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
        if (previewObject == null)
        {
            // The user has selected a mine type and should now click a conveyor
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Ground"))
            {
                GameObject newMine = InstantiateSelectedObject(hit.point);


                newMine.transform.SetParent(resourceObject);
                Objects.availableResources.Enqueue(newMine.transform);


                selectedMineTypeIndex = -1; // Reset the selected mine type index
            }


        }
        else if (previewObject != null)
        {

            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Grounda"))
            {
                InstantiateSelectedObject(hit.point);
            }

            previewObject = null;
        }
    }

    // Instantiate the selected object at the given position
    private GameObject InstantiateSelectedObject(Vector3 position)
    {
        return Instantiate(selectedObject, position, Quaternion.identity);
    }

    // Select an object from the build menu
    public void SelectObject(int index)
    {
        if (index >= 0 && index < buildObjects.Length)
        {

            // Set the selected mine type index and return
            previewObject = null;
            selectedMineTypeIndex = index;
            selectedObject = buildObjects[index];
            selectedObjectIndex = index;
            return;

        }
    }

}