using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralProcessingMachine : MonoBehaviour
{
    public GameObject[] prefabsToInstantiate;
    private GameObject refinedObject;
    List<GameObject> gameObjects;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Gold Mine")
        {
            Destroy(other.gameObject);
            refinedObject = Resources.Load<GameObject>("Prefabs/Refined Gold");
            Instantiate(refinedObject, other.gameObject.transform.position, Quaternion.identity);
        }
    }
}
