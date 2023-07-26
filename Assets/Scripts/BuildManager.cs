using UnityEngine;
using UnityEngine.EventSystems;

public class BuildManager : MonoBehaviour
{
    [SerializeField] GameObject[] buildObjects;
    private GameObject selectedObject;
    private GameObject previewObject;
    private bool isPreviewing;
    private Ray ray;
    private RaycastHit hit;

    private void Update()
    {
        HandlePreview();

        if (Input.GetMouseButtonDown(0))
        {
            PlaceObject();
        }
    }

    private void HandlePreview()
    {
        if (isPreviewing)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Destroy(previewObject);
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

    private void MovePreviewObject(Vector3 targetPosition)
    {
        previewObject.transform.position = targetPosition;
    }

    private void RotatePreviewObject()
    {
        previewObject.transform.Rotate(Vector3.up, 90f);
    }

    private void PlaceObject()
    {
        if (previewObject != null)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Ground"))
            {
                InstantiateSelectedObject(hit.point);
            }

            previewObject = null;
            isPreviewing = false;
        }
    }

    private void InstantiateSelectedObject(Vector3 position)
    {
        Instantiate(selectedObject, position, Quaternion.identity);
    }

    public void SelectObject(int index)
    {
        if (index >= 0 && index < buildObjects.Length)
        {
            selectedObject = buildObjects[index];
            DestroyPreviewObject();
            CreatePreviewObject();
            isPreviewing = true;
        }
        EventSystem.current.SetSelectedGameObject(null); // cancel keyboard (pressing space etc.)
    }

    private void DestroyPreviewObject()
    {
        if (previewObject != null)
        {
            Destroy(previewObject);
        }
    }

    private void CreatePreviewObject()
    {
        previewObject = Instantiate(selectedObject);
    }
}
