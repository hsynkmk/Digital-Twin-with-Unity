using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComplexMachine : MonoBehaviour
{

    public GameObject phone; 

    private int ironCount = 0;
    private int copperCount = 0;
    private int chipCount = 0;

    public int requiredIron = 1;
    public int requiredCopper = 1;
    public int requiredChip = 1;

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Refined Iron") || collision.gameObject.CompareTag("Refined Copper") || collision.gameObject.CompareTag("Chip"))
        {
          
            if (collision.gameObject.CompareTag("Refined Iron"))
                ironCount++;
            else if (collision.gameObject.CompareTag("Refined Copper"))
                copperCount++;
            else if (collision.gameObject.CompareTag("Chip"))
                chipCount++;

            Destroy(collision.gameObject);

            if (ironCount >= requiredIron && copperCount >= requiredCopper && chipCount >= requiredChip)
            {
                ProducePhone();
            }
        }
    }

    private void ProducePhone()
    {
        Vector3 spawnPosition = transform.position + new Vector3(0, 1, 4);
        Instantiate(phone, spawnPosition, Quaternion.identity);
        ironCount = 0;
        copperCount = 0;
        chipCount = 0;
    }
}
