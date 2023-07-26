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
            StartCoroutine(PerformConversion());
        }
    }

    IEnumerator PerformConversion()
    {
        Vector3 vector3 = transform.position;
        Quaternion quaternion = Quaternion.identity;

        yield return new WaitForSeconds(3);

        Vector3 spawnPosition = vector3 + new Vector3(4, 1, 0);
        Instantiate(refinedObject, spawnPosition, quaternion);
        Destroy(gameObject);
    }
}
