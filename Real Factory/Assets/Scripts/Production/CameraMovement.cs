using UnityEngine;


public class CameraController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f; // Speed at which the camera moves
    private Transform cameraTransform; // Reference to the camera's transform

    private void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        MoveCamera();
    }

    private void MoveCamera()
    {
        // Get the horizontal and vertical input values (A/D and W/S keys)
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate the new position of the camera
        Vector3 newPosition = cameraTransform.position;
        newPosition += cameraTransform.forward * verticalInput * moveSpeed * Time.deltaTime; // Move forward/backward
        newPosition += cameraTransform.right * horizontalInput * moveSpeed * Time.deltaTime; // Move left/right

        // Keep the y-position unchanged
        newPosition.y = cameraTransform.position.y;

        // Apply the new position to the camera
        cameraTransform.position = newPosition;
    }
}
