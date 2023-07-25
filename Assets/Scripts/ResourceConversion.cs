using System.Collections;
using UnityEngine;

public class ResourceConversion : MonoBehaviour
{
    [SerializeField] GameObject refinedObject;
    //[SerializeField] int conversionTime = 5;

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Mineral Machine"))
        {
            Destroy(gameObject);
            PerformConversion();
        }
    }

    private void PerformConversion()
    {

            Vector3 spawnPosition = transform.position + new Vector3(4, 1, 0);
            Instantiate(refinedObject, spawnPosition, Quaternion.identity);

    }
}
