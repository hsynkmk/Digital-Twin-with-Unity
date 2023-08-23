using Unity.VisualScripting;
using UnityEngine;

public class Initilizer : MonoBehaviour
{
    [SerializeField] private Transform destinationTransform;
    [SerializeField] private Transform productTransform;
    [SerializeField] private Transform parkTransform;

    private void Awake()
    {
        Objects.productTransform = productTransform;
        Objects.MakeAllAvailable();

        Park.parkTransform = parkTransform;
        Park.MakeAllUnavailable();
    }
}