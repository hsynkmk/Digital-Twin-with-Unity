using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralProcessingMachine : MonoBehaviour
{
    [SerializeField] GameObject[] mineralsToProcess;
    [SerializeField] GameObject[] refinedObjects;
    [SerializeField] int conversionTime = 3;
    private Light processingLight;

    private void Start()
    {
        processingLight = GetComponentInChildren<Light>();
    }

    private void OnCollisionEnter(Collision other)
    {
        // Check if the collided object is one of the minerals to process
        int index = GetMineralIndex(other.gameObject);
        if (index != -1)
        {
            Destroy(other.gameObject);
            StartCoroutine(PerformConversion(index, other.contacts[0].point, other.contacts[0].normal));
        }
    }

    // Get the index of the collided object in the mineralsToProcess array
    private int GetMineralIndex(GameObject obj)
    {
        for (int i = 0; i < mineralsToProcess.Length; i++)
        {
            if (obj.tag.Equals(mineralsToProcess[i].tag))
            {
                return i;
            }
        }
        return -1; // Object not found in the mineralsToProcess array
    }

    IEnumerator PerformConversion(int mineralIndex, Vector3 collisionPoint, Vector3 collisionNormal)
    {
        Quaternion rotation = Quaternion.identity;
        Vector3 offset = collisionNormal * 3f;
        Vector3 spawnPosition = collisionPoint + offset + new Vector3(0,0,0);
        GameObject refinedResult = refinedObjects[mineralIndex];

        // Blink the light three times
        for (int i = 0; i < 3; i++)
        {
            processingLight.enabled = true;
            yield return new WaitForSeconds(0.2f); // Light on for 0.2 seconds
            processingLight.enabled = false;
            yield return new WaitForSeconds(0.2f); // Light off for 0.2 seconds
        }

        yield return new WaitForSeconds(conversionTime);

        Instantiate(refinedResult, spawnPosition, rotation);
    }
}
