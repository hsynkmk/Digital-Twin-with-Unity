using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralProcessingMachine : MonoBehaviour
{
    [SerializeField] private GameObject[] mineralsToProcess; // Array of mineral prefabs to process
    [SerializeField] private GameObject[] refinedObjects; // Array of refined object prefabs
    [SerializeField] private float blinkDuration = 0.2f; // Duration for each light blink
    [SerializeField] private int blinkCount = 3; // Number of times to blink the light
    [SerializeField] private float conversionTime = 2f; // Duration of the conversion process

    private void OnCollisionEnter(Collision other)
    {
        // Check if the collided object is a mineral to process
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
            if (obj.CompareTag(mineralsToProcess[i].tag))
            {
                return i;
            }
        }
        return -1; // Return -1 if the collided object is not found in the mineralsToProcess array
    }

    // Perform the conversion process for the given mineral index
    private IEnumerator PerformConversion(int mineralIndex, Vector3 collisionPoint, Vector3 collisionNormal)
    {
        Vector3 offset = collisionNormal * 3f;
        Vector3 spawnPosition = collisionPoint + offset;
        GameObject refinedResult = refinedObjects[mineralIndex];

        Light processingLight = GetComponentInChildren<Light>();
        // Blink the light for the specified number of times
        for (int i = 0; i < blinkCount; i++)
        {
            processingLight.enabled = true;
            yield return new WaitForSeconds(blinkDuration);
            processingLight.enabled = false;
            yield return new WaitForSeconds(blinkDuration);
        }

        // Wait for the conversion time
        yield return new WaitForSeconds(conversionTime);

        // Instantiate the refined object at the spawn position
        Instantiate(refinedResult, spawnPosition, Quaternion.identity);
    }
}
