using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] float size = 1f;

    // Get the nearest point on the grid to the given position
    public Vector3 GetNearestPointOnGrid(Vector3 position)
    {
        // Translate position relative to the grid origin
        position -= transform.position;

        // Round the position to the nearest grid cell index
        int xCount = Mathf.RoundToInt(position.x / size);
        int yCount = Mathf.RoundToInt(position.y / size);
        int zCount = Mathf.RoundToInt(position.z / size);

        // Calculate the result position in world space based on the rounded grid cell index
        Vector3 result = new Vector3(
            (float)xCount * size,
            (float)yCount * size,
            (float)zCount * size);

        // Translate the result position back to the grid's world position
        result += transform.position;

        return result;
    }

    // Visualize the grid points in the scene view
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        // Loop through the grid's cells and draw a sphere at each grid point
        for (float x = -20; x < 20; x += size)
        {
            for (float z = -20; z < 20; z += size)
            {
                // Get the nearest grid point to the current position
                var point = GetNearestPointOnGrid(new Vector3(x, 0f, z));
                // Draw a sphere at the grid point to visualize it
                Gizmos.DrawSphere(point, 0.08f);
            }
        }
    }
}
