using System.Collections;
using UnityEngine;

public class ChipMachine : MonoBehaviour
{
    [SerializeField] private GameObject silicon; // Reference to the silicon object
    [SerializeField] private GameObject chip; // Reference to the chip prefab
    [SerializeField] private float blinkDuration = 0.2f; // Duration for each light blink
    [SerializeField] private int blinkCount = 3; // Number of times to blink the light
    [SerializeField] private float conversionTime = 2f; // Time it takes to convert silicon into a chip

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Silicon"))
        {
            Destroy(other.gameObject);
            StartCoroutine(PerformConversion(other.contacts[0].point, other.contacts[0].normal));
        }
    }

    private IEnumerator PerformConversion(Vector3 collisionPoint, Vector3 collisionNormal)
    {
        // Offset to spawn the chip slightly away from the collision point
        Vector3 offset = collisionNormal * 3f + new Vector3(-1, 0, 0);
        // Position to spawn the chip
        Vector3 spawnPosition = collisionPoint + offset;

        // Blink the light multiple times to indicate processing
        Light processingLight = GetComponentInChildren<Light>();

        for (int i = 0; i < blinkCount; i++)
        {
            processingLight.enabled = true;
            yield return new WaitForSeconds(blinkDuration);
            processingLight.enabled = false;
            yield return new WaitForSeconds(blinkDuration);
        }
        // Wait for the conversion time
        yield return new WaitForSeconds(conversionTime);

        // Create a new chip at the spawn position
        Instantiate(chip, spawnPosition, Quaternion.identity); 
    }
}
