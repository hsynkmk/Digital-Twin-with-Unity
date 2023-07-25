using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConveyorMover : MonoBehaviour
{
    // The speed at which the conveyor moves
    public float speed = 1f;

    // Enumeration for different movement directions of the conveyor
    enum Direction { Forward, Backward, Left, Right }

    // The selected direction of conveyor movement
    [SerializeField] Direction direction;

    // Reference to the Rigidbody component of the conveyor
    Rigidbody beltRigidbody;

    // Reference to the Material used for the conveyor's texture
    Material beltMaterial;

    void Start()
    {
        // Get the references to the Rigidbody and Material components
        beltRigidbody = GetComponent<Rigidbody>();
        beltMaterial = GetComponent<Renderer>().material;
    }

    void Update()
    {
        // Scroll the conveyor texture
        ScrollTexture();
    }

    void FixedUpdate()
    {
        // Move the conveyor in the selected direction
        MoveConveyor();
    }

    // Move the conveyor based on the selected direction
    void MoveConveyor()
    {
        // Get the current position of the conveyor
        Vector3 currentPos = beltRigidbody.position;

        // Move the conveyor in the selected direction based on the speed and deltaTime
        switch (direction)
        {
            case Direction.Forward:
                beltRigidbody.position += Vector3.back * speed * Time.fixedDeltaTime;
                break;
            case Direction.Backward:
                beltRigidbody.position += Vector3.forward * speed * Time.fixedDeltaTime;
                break;
            case Direction.Left:
                beltRigidbody.position += Vector3.right * speed * Time.fixedDeltaTime;
                break;
            case Direction.Right:
                beltRigidbody.position += Vector3.left * speed * Time.fixedDeltaTime;
                break;
            default:
                break;
        }

        beltRigidbody.MovePosition(currentPos);
    }

    // Scroll the conveyor texture to give the appearance of movement
    void ScrollTexture()
    {
        // Calculate the new texture offset based on the speed and deltaTime
        Vector2 offset = beltMaterial.mainTextureOffset;
        offset += Vector2.left * speed * Time.deltaTime / beltMaterial.mainTextureScale.y;
        beltMaterial.mainTextureOffset = offset;
    }
}
