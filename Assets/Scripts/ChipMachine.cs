using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipMachine : MonoBehaviour
{
    [SerializeField] GameObject silicon;
    [SerializeField] GameObject chip;
    [SerializeField] int conversionTime = 3;


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag.Equals("Silicon"))
        {
            Destroy(other.gameObject);
            StartCoroutine(PerformConversion(other.contacts[0].point, other.contacts[0].normal));
        }
    }


    IEnumerator PerformConversion(Vector3 collisionPoint, Vector3 collisionNormal)
    {
        Quaternion rotation = Quaternion.identity;
        Vector3 offset = collisionNormal * 5f;
        Vector3 spawnPosition = collisionPoint + offset + new Vector3(0, 0, 0);

        // Blink the light three times
    /*    for (int i = 0; i < 3; i++)
        {
            processingLight.enabled = true;
            yield return new WaitForSeconds(0.2f); // Light on for 0.2 seconds
            processingLight.enabled = false;
            yield return new WaitForSeconds(0.2f); // Light off for 0.2 seconds
        }*/

        yield return new WaitForSeconds(conversionTime);

        Instantiate(chip, spawnPosition, rotation);
    }
}
