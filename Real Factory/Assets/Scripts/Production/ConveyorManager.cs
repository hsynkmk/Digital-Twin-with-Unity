using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorManager : MonoBehaviour
{
    // Reference to the ConveyorMover component
    private ConveyorMover conveyorMover;

    private void Start()
    {
        // Get the ConveyorMover component attached to the conveyor object
        conveyorMover = GetComponentInChildren<ConveyorMover>();

        if (conveyorMover == null)
        {
            Debug.LogError("ConveyorMover component not found.");
            return;
        }

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
        
    }

    // Enable the conveyor's movement
    private void EnableConveyor()
    {
        conveyorMover.enabled = true;
    }

    // Disable the conveyor's movement
    private void DisableConveyor()
    {
        conveyorMover.enabled = false;
    }
}
