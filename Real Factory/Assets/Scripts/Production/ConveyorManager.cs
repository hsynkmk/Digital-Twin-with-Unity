using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorManager : MonoBehaviour
{
    [SerializeField] Collider topCollider;

    private void Start()
    {
        // Disable the conveyor at the start
        DisableConveyor();
    }

    // Enable the conveyor when an object enters the trigger collider
    private void OnTriggerEnter(Collider other)
    {
        EnableConveyor();
    }

    // Disable the conveyor when an object exits the trigger collider
    private void OnTriggerExit(Collider other)
    {
        DisableConveyor();
        Debug.Log("sdssd");
    }

    // Enable the conveyor's movement
    private void EnableConveyor()
    {
        foreach (ConveyorMover Convey in GetComponentsInChildren<ConveyorMover>())
        {
            Convey.enabled = true;
        }

        foreach (SideConveyorMover Convey in GetComponentsInChildren<SideConveyorMover>())
        {
            Convey.enabled = true;
        }
    }

    // Disable the conveyor's movement
    private void DisableConveyor()
    {
        foreach (ConveyorMover Convey in GetComponentsInChildren<ConveyorMover>())
        {
            Convey.enabled = false;
        }

        foreach (SideConveyorMover Convey in GetComponentsInChildren<SideConveyorMover>())
        {
            Convey.enabled = false;
        }
    }
}
