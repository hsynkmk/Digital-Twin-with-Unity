using System;
using UnityEngine;

/// <summary>
///     A simple free camera to be added to a Unity game object.
///     Keys:
///     wasd / arrows	- movement
///     q/e 			- down/up (local space)
///     r/f 			- up/down (world space)
///     pageup/pagedown	- up/down (world space)
///     hold shift		- enable fast movement mode
///     right mouse  	- enable free look
///     mouse			- free look / rotation
/// </summary>
public class FreeCam : MonoBehaviour
{
    // Normal speed of camera movement.
    [SerializeField] float m_MovementSpeed = 10f;

    // Speed of camera movement when shift is held down,
    [SerializeField] float m_FastMovementSpeed = 100f;

    // Sensitivity for free look.
    [SerializeField] float m_FreeLookSensitivity = 3f;

    // Amount to zoom the camera when using the mouse wheel.
    [SerializeField] float m_ZoomSensitivity = 10f;

    // Amount to zoom the camera when using the mouse wheel (fast mode).
    [SerializeField] float m_FastZoomSensitivity = 50f;


    // Set to true when free looking (on right mouse button).
    bool m_Looking;

    void Update()
    {
        var fastMode = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        var movementSpeed = fastMode ? m_FastMovementSpeed : m_MovementSpeed;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            transform.position = transform.position + -transform.right * movementSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            transform.position = transform.position + transform.right * movementSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            transform.position = transform.position + transform.forward * movementSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            transform.position = transform.position + -transform.forward * movementSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.R))
            transform.position = transform.position + Vector3.up * movementSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.F))
            transform.position = transform.position + -Vector3.up * movementSpeed * Time.deltaTime;

        if (m_Looking)
        {
            var newRotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * m_FreeLookSensitivity;
            var newRotationY = transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * m_FreeLookSensitivity;
            transform.localEulerAngles = new Vector3(newRotationY, newRotationX, 0f);
        }

        var axis = Input.GetAxis("Mouse ScrollWheel");
        if (axis != 0)
        {
            var zoomSensitivity = fastMode ? m_FastZoomSensitivity : m_ZoomSensitivity;
            transform.position = transform.position + transform.forward * axis * zoomSensitivity;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
            StartLooking();
        else if (Input.GetKeyUp(KeyCode.Mouse1)) StopLooking();
    }

    void OnDisable()
    {
        StopLooking();
    }

    // Enable free looking.
    void StartLooking()
    {
        m_Looking = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Disable free looking.
    void StopLooking()
    {
        m_Looking = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}