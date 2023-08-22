using Unity.VisualScripting;
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

        Objects.productTransform = productTransform;
        Objects.MakeAllAvailable();

        Park.parkTransform = parkTransform;
        Park.MakeAllUnavailable();
    }
}