using System.Collections;
using TMPro;
using UnityEngine;

public class PhoneMachine : MonoBehaviour
{
    [SerializeField] private GameObject phonePrefab;
    [SerializeField] private Transform objectProduct;
    [SerializeField] private int requiredIron = 1;
    [SerializeField] private int requiredCopper = 1;
    [SerializeField] private int requiredChip = 1;
    [SerializeField] private float blinkDuration = 0.2f; // Duration for each light blink
    [SerializeField] private int blinkCount = 3; // Number of times to blink the light
    [SerializeField] private float conversionTime = 2f; // Duration of the conversion process
    [SerializeField] TextMeshProUGUI phoneCountText;
    private int ironCount = 0;
    private int copperCount = 0;
    private int chipCount = 0;

    private void Start()
    {
        // Get reference to the TextMeshProUGUI component for phone count display
    }

    private void OnCollisionEnter(Collision other)
    {
        if (IsRefinedResource(other.gameObject))
        {
            IncrementResourceCount(other.gameObject);

            Destroy(other.gameObject);

            if (HasEnoughResources())
            {
                StartCoroutine(ProducePhone());
            }
        }
    }

    private bool IsRefinedResource(GameObject obj)
    {
        // Check if the object is a refined resource by comparing tags
        return obj.CompareTag("Refined Iron") || obj.CompareTag("Refined Copper") || obj.CompareTag("Chip");
    }

    private void IncrementResourceCount(GameObject obj)
    {
        // Increment the respective resource count based on the object's tag
        if (obj.CompareTag("Refined Iron"))
        {
            ironCount++;
        }
        else if (obj.CompareTag("Refined Copper"))
        {
            copperCount++;
        }
        else if (obj.CompareTag("Chip"))
        {
            chipCount++;
        }
    }

    private void DecrementResourceCount()
    {
        // Decrease the resource counts by 1 after producing a phone
        ironCount--;
        copperCount--;
        chipCount--;
    }

    private bool HasEnoughResources()
    {
        // Check if there are enough resources to produce a phone based on the required amounts
        return ironCount >= requiredIron && copperCount >= requiredCopper && chipCount >= requiredChip;
    }

    private IEnumerator ProducePhone()
    {
        // Spawn position for the phone
        Vector3 spawnPosition = transform.position + new Vector3(0, 2, 3);
        //Vector3 spawnPosition = new Vector3(10, 2, -15);

        // Get the processing light component
        Light processingLight = GetComponentInChildren<Light>();

        // Blink the light for a specified number of times
        for (int i = 0; i < blinkCount; i++)
        {
            processingLight.enabled = true;
            yield return new WaitForSeconds(blinkDuration); // Light on for a specified duration
            processingLight.enabled = false;
            yield return new WaitForSeconds(blinkDuration); // Light off for a specified duration
        }

        yield return new WaitForSeconds(conversionTime); // Wait for the conversion process

        // Instantiate a new phone at the spawn position and increase the phone count
        GameObject newPhone = Instantiate(phonePrefab, spawnPosition, Quaternion.identity);

        newPhone.transform.SetParent(objectProduct);
        Objects.availableResources.Enqueue(newPhone.transform);

        //Debug.Log(Objects.availableProducts.Count);

        int phoneCount = int.Parse(phoneCountText.text);
        phoneCount++;
        phoneCountText.text = phoneCount.ToString();

        DecrementResourceCount(); // Decrease the resource counts after producing a phone
    }
}
