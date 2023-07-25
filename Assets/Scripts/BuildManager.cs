using UnityEngine;
using UnityEngine.EventSystems;

public class BuildManager : MonoBehaviour
{
    [SerializeField] GameObject[] buildObjects;
    private GameObject selectedObject;
    private GameObject previewObject;
    private bool isPreviewing;

    private void Update()
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
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.CompareTag("Ground"))
                    {
                        Vector3 targetPosition = hit.point;
                        previewObject.transform.position = targetPosition;
                        if (Input.GetKeyDown(KeyCode.Space))
                        {
                            previewObject.transform.Rotate(Vector3.up, 90f);
                        }
                    }
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (previewObject != null)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    isPreviewing = false;

                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.CompareTag("Ground"))
                        {
                            Instantiate(previewObject, hit.point, Quaternion.identity);
                        }
                    }

                    previewObject = null;
                }
            }
        }
    }

    public void SelectObject(int index)
    {
        if (index >= 0 && index < buildObjects.Length)
        {
            selectedObject = buildObjects[index];
            if (previewObject != null)
            {
                Destroy(previewObject);
            }
            previewObject = Instantiate(selectedObject);
            isPreviewing = true;
        }
        EventSystem.current.SetSelectedGameObject(null); // cancel keyboard (pressing space etc.)
    }
}
