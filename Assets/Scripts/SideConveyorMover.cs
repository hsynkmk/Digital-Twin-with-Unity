using UnityEngine;

public class SideConveyorMover : MonoBehaviour
{
    [SerializeField] private GameObject topBelt;
    private float eulerSpeed;
    private Rigidbody sideRigidbody;

    private void Start()
    {
        // Get the speed of the top belt from the ConveyorMover script attached to the 'topBelt' GameObject
        float beltSpeed = topBelt.GetComponent<ConveyorMover>().speed;
        // Calculate the rotation speed in radians per second based on the belt speed
        eulerSpeed = -(2f * Mathf.PI) / (51f * beltSpeed);
        // Get the Rigidbody component of the side cylinder belts
        sideRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // Rotate the side cylinder belts
        RotateBelts();
    }

    // Rotate the side cylinder belts based on the calculated eulerSpeed
    private void RotateBelts()
    {
        // Calculate the new rotation based on the eulerSpeed
        Quaternion rotation = Quaternion.Euler(0f, eulerSpeed * Mathf.Rad2Deg, 0f);
        // Apply the new rotation to the side cylinder belts using MoveRotation to avoid physics issues
        sideRigidbody.MoveRotation(sideRigidbody.rotation * rotation);
    }
}
