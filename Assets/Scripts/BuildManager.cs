using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public GameObject[] buildObjects; 
    private GameObject selectedObject; 

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            if (selectedObject != null) 
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.CompareTag("Ground"))
                    {
                        Instantiate(selectedObject, hit.point, Quaternion.identity);
                    }
                }

                selectedObject = null;
            }
        }
    }

    public void SelectObject(int index)
    {
        if (index >= 0 && index < buildObjects.Length)
        {
            selectedObject = buildObjects[index];
        }
    }
}
