using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class NavMeshBaker : MonoBehaviour
{
    NavMeshSurface surface;
    // Start is called before the first frame update
    void Start()
    {
        surface = transform.GetComponent<NavMeshSurface>();
    }

    // Update is called once per frame
    void Update()
    {
        surface.BuildNavMesh();
    }
}
