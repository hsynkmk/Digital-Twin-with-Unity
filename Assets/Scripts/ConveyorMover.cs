using System.Collections;
using UnityEngine;

public class ConveyorMover : MonoBehaviour
{
    public float speed = 1f; // The speed at which the conveyor moves

    private enum Direction { Forward, Backward, Left, Right } // Enumeration for different movement directions of the conveyor
    private Direction direction; // The selected direction of conveyor movement
    private Rigidbody beltRigidbody; // Reference to the Rigidbody component of the conveyor
    private Material beltMaterial; // Reference to the Material used for the conveyor's texture

    private void Start()
    {
        // Get the references to the Rigidbody and Material components
        beltRigidbody = GetComponent<Rigidbody>();
        beltMaterial = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        // Scroll the conveyor texture
        ScrollTexture();

        // Determine the direction based on the parent's rotation
        UpdateMovementDirection();
    }

    private void FixedUpdate()
    {
        // Move the conveyor in the selected direction
        MoveConveyor();
    }

    // Move the conveyor based on the selected direction
    private void MoveConveyor()
    {
        // Get the current position of the conveyor
        Vector3 currentPos = beltRigidbody.position;

        // Move the conveyor in the selected direction based on the speed and deltaTime
        Vector3 movement = Vector3.zero;
        switch (direction)
        {
            case Direction.Forward:
                movement = Vector3.back;
                break;
            case Direction.Backward:
                movement = Vector3.forward;
                break;
            case Direction.Left:
                movement = Vector3.right;
                break;
            case Direction.Right:
                movement = Vector3.left;
                break;
        }

        beltRigidbody.position += movement * speed * Time.fixedDeltaTime;

        // Ensure the Rigidbody position is updated correctly
        beltRigidbody.MovePosition(currentPos);
    }

    // Scroll the conveyor texture to give the appearance of movement
    private void ScrollTexture()
    {
        // Calculate the new texture offset based on the speed and deltaTime
        Vector2 offset = beltMaterial.mainTextureOffset;
        offset += Vector2.left * speed * Time.deltaTime / beltMaterial.mainTextureScale.y;
        beltMaterial.mainTextureOffset = offset;
    }

    // Determine the direction of conveyor movement based on the parent's rotation
    private void UpdateMovementDirection()
    {
        // Get the parent's rotation and convert it to positive values
        Vector3 parentRotation = transform.parent.eulerAngles;
        parentRotation.y = Mathf.Repeat(parentRotation.y, 360f);

        // Divide the parent's rotation by 90 to get a value between 0 and 4
        int rotationDivision = (int)parentRotation.y / 90;

        // Set the direction based on the value of rotationDivision
        switch (rotationDivision)
        {
            case 0:
                direction = Direction.Right;
                break;
            case 1:
                direction = Direction.Backward;
                break;
            case 2:
                direction = Direction.Left;
                break;
            case 3:
                direction = Direction.Forward;
                break;
            case 4:
                direction = Direction.Right;
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Center the collided object on the conveyor

        Vector3 conveyorCenter = transform.position;
        Vector3 objectBoundsCenter = collision.collider.bounds.center;

        // Calculate the offset to move the object's pivot to the center of the conveyor
        Vector3 offset = CalculateCenteringOffset(conveyorCenter, objectBoundsCenter);

        // Move the object's transform by the offset to place its pivot on the center of the conveyor
        collision.transform.position += offset;

    }

    private Vector3 CalculateCenteringOffset(Vector3 conveyorCenter, Vector3 objectBoundsCenter)
    {
        // Calculate the offset in the direction of the conveyor's movement
        Vector3 conveyorDirection = transform.forward;
        Vector3 directionToCenter = conveyorCenter - objectBoundsCenter;
        float distanceAlongConveyor = Vector3.Dot(directionToCenter, conveyorDirection);
        Vector3 offset = conveyorDirection * distanceAlongConveyor;

        return offset;
    }
}
