using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus;

public class CanvasFollowr : MonoBehaviour
{
    [Header("Target Settings")]
    [Tooltip("The VR camera that this object will follow.")]
    public Transform vrCamera;

    [Header("Position Settings")]
    [Tooltip("Desired distance in front of the camera.")]
    public float distance = 3.0f;
    [Tooltip("Damping time for position smoothing.")]
    public float positionSmoothTime = 0.3f;

    [Header("Rotation Settings")]
    [Tooltip("Damping time for rotation smoothing.")]
    public float rotationSmoothTime = 0.3f;
    [Tooltip("Lock rotation to specific axes (e.g., Y-axis only).")]
    public bool lockYRotation = true;

    private Vector3 velocity = Vector3.zero;
    private Quaternion rotationVelocity = Quaternion.identity;
    void Start()
    {
        vrCamera = FindObjectOfType<OVRCameraRig>().centerEyeAnchor;

    }

    void LateUpdate()
    {
        if (vrCamera == null) return;

        // Calculate target position in front of the camera
        Vector3 targetPosition = vrCamera.TransformPoint(new Vector3(0, 0, distance));

        // Smoothly move towards the target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, positionSmoothTime);

        // Calculate target rotation (facing the camera)
        Quaternion targetRotation = Quaternion.LookRotation(vrCamera.position - transform.position, Vector3.up);

        // If locking Y rotation, constrain the rotation
        if (lockYRotation)
        {
            Vector3 euler = targetRotation.eulerAngles;
            euler.x = 0; // Lock X rotation
            euler.z = 0; // Lock Z rotation
            targetRotation = Quaternion.Euler(euler);
        }

        // Smoothly rotate towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime / rotationSmoothTime);
    }
}
