using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConveyorMover : MonoBehaviour
{
    public float speed = 1f;// The speed at which the conveyor moves
    private enum Direction { Forward, Backward, Left, Right }// Enumeration for different movement directions of the conveyor
    private Direction direction;// The selected direction of conveyor movement
    private Rigidbody beltRigidbody;// Reference to the Rigidbody component of the conveyor
    private Material beltMaterial;// Reference to the Material used for the conveyor's texture

    void Start()
    {
        // Get the references to the Rigidbody and Material components
        beltRigidbody = GetComponent<Rigidbody>();
        beltMaterial = GetComponent<Renderer>().material;

        // Get the parent's rotation and convert it to positive values
        Vector3 parentRotation = transform.parent.eulerAngles;

        // Convert the parent's rotation to a value between 0 and 360
        while (parentRotation.y < 0f)
        {
            parentRotation.y += 360f;
        }

        // Divide the parent's rotation by 90 to get a value between 0 and 4
        int rotationDivision = (int)parentRotation.y / 90;

        // Set the direction based on the value of divide
        switch (rotationDivision)
        {
            case 0:
                direction = Direction.Right; break;
            case 1:
                direction = Direction.Backward; break;
            case 2:
                direction = Direction.Left; break;
            case 3:
                direction = Direction.Forward; break;
            case 4:
                direction = Direction.Right; break;
        }
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
                beltRigidbody.position += Vector3.back * speed * Time.fixedDeltaTime; break;
            case Direction.Backward:
                beltRigidbody.position += Vector3.forward * speed * Time.fixedDeltaTime; break;
            case Direction.Left:
                beltRigidbody.position += Vector3.right * speed * Time.fixedDeltaTime; break;
            case Direction.Right:
                beltRigidbody.position += Vector3.left * speed * Time.fixedDeltaTime; break;
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
