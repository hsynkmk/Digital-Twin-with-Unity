using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PhoneMachine : MonoBehaviour
{
    public GameObject phone;
    private Light processingLight;
    private TextMeshProUGUI phoneCountText;
    private int phoneCount;
    [SerializeField] int conversionTime = 3;

    private int ironCount = 0;
    private int copperCount = 0;
    private int chipCount = 0;

    public int requiredIron = 1;
    public int requiredCopper = 1;
    public int requiredChip = 1;

    private void Start()
    {
        processingLight = GetComponentInChildren<Light>();
        phoneCountText = GameObject.FindGameObjectWithTag("Phone Count").GetComponent<TextMeshProUGUI>();
    }

    private void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.CompareTag("Refined Iron") || other.gameObject.CompareTag("Refined Copper") || other.gameObject.CompareTag("Chip"))
        {
          
            if (other.gameObject.CompareTag("Refined Iron"))
                ironCount++;
            else if (other.gameObject.CompareTag("Refined Copper"))
                copperCount++;
            else if (other.gameObject.CompareTag("Chip"))
                chipCount++;

            Destroy(other.gameObject);

            if (ironCount >= requiredIron && copperCount >= requiredCopper && chipCount >= requiredChip)
            {
                StartCoroutine(ProducePhone(other.contacts[0].point, other.contacts[0].normal));
            }
        }
    }

    IEnumerator ProducePhone(Vector3 collisionPoint, Vector3 collisionNormal)
    {
        Vector3 offset = collisionNormal * 3f;
        Vector3 spawnPosition = collisionPoint + offset;

        for (int i = 0; i < 3; i++)
        {
            processingLight.enabled = true;
            yield return new WaitForSeconds(0.2f); // Light on for 0.2 seconds
            processingLight.enabled = false;
            yield return new WaitForSeconds(0.2f); // Light off for 0.2 seconds
        }

        yield return new WaitForSeconds(conversionTime);

        Instantiate(phone, spawnPosition, Quaternion.identity);
        phoneCount = int.Parse(phoneCountText.text);
        phoneCount++;
        phoneCountText.text = phoneCount.ToString();
        ironCount = 0;
        copperCount = 0;
        chipCount = 0;
    }
}