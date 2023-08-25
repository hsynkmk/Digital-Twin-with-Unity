using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryLocations : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        //collision.transform.parent = transform;

        collision.transform.localPosition += new Vector3(1.5f, 2, 0);
    }
}
