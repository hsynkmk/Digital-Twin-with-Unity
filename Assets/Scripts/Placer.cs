using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePlacer : MonoBehaviour
{
    Grid grid;
    [SerializeField] GameObject prefab;

    private void Awake()
    {
        grid = FindObjectOfType<Grid>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hitInfo))
            {
                PlaceObjectNear(hitInfo.point);
            }
        }
    }

    private void PlaceObjectNear(Vector3 clickPoint)
    {
        var finalPosition = grid.GetNearestPointOnGrid(clickPoint);
        // Instantiate the conveyor prefab at the finalPosition
        Instantiate(prefab, finalPosition, Quaternion.identity);
    }
}
