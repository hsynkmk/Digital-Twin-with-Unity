using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initilizer : MonoBehaviour
{
    [SerializeField] private Transform destinationTransform;
    [SerializeField] private Transform productTransform;
    [SerializeField] private Transform parkTransform;

    private void Awake()
    {
        Destination.destinationTransform = destinationTransform;
        Destination.MakeAllAvailable();

        Product.productTransform = productTransform;
        Product.MakeAllAvailable();

        Park.parkTransform = parkTransform;
        Park.MakeAllUnavailable();
    }
}
