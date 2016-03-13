using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class GuardsCameraDriver : CameraDriver
{
    private const float yOffset = 10.0f;
    private const float zOffset = -5.0f;

    private const float DAMP_TIME = 0.2f;
    private const float SCREEN_EDGE_BUFFER = 4f;
    private const float MIN_SIZE = 6.5f;

    private Camera camera;
    private float zoomSpeed;
    private Vector3 moveVelocity; 
    private Vector3 desiredPosition;

    public GuardsCameraDriver(GameObject cameraInstance)
        : base(cameraInstance)
    {
        this.camera = cameraInstance.GetComponent<Camera>();
    }

    public override void Update()
    {
        Move();
        //Zoom();
    }

    private void Move()
    {
        // Find the average position of the targets.
        FindAveragePosition();

        // Smoothly transition to that position.
        this.instance.transform.position = Vector3.SmoothDamp(this.instance.transform.position, desiredPosition, ref moveVelocity, DAMP_TIME);
    }

    private void FindAveragePosition()
    {
        Vector3 averagePos = new Vector3();

        // Go through all the targets and add their positions together.
        foreach(NPCDriver target in GameManager.Guards)
        {
            // Add to the average and increment the number of targets in the average.
            averagePos += target.Instance.transform.position;
        }

        // If there are targets divide the sum of the positions by the number of them to find the average.
        averagePos /= GameManager.Guards.Count;

        // Keep the same y value.
        averagePos.y = yOffset;
        averagePos.z += zOffset;

        // The desired position is the average position;
        desiredPosition = averagePos;
    }

    private void Zoom()
    {
        // Find the required size based on the desired position and smoothly transition to that size.
        float requiredSize = FindRequiredSize();
        this.camera.orthographicSize = Mathf.SmoothDamp(this.camera.orthographicSize, requiredSize, ref zoomSpeed, DAMP_TIME);
    }

    private float FindRequiredSize()
    {
        // Find the position the camera rig is moving towards in its local space.
        Vector3 desiredLocalPos = this.instance.transform.InverseTransformPoint(desiredPosition);

        // Start the camera's size calculation at zero.
        float size = 0f;

        // Go through all the targets...
        foreach(NPCDriver target in GameManager.Guards)
        {
            // Otherwise, find the position of the target in the camera's local space.
            Vector3 targetLocalPos = target.Instance.transform.InverseTransformPoint(target.Instance.transform.position);

            // Find the position of the target from the desired position of the camera's local space.
            Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

            // Choose the largest out of the current size and the distance of the tank 'up' or 'down' from the camera.
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));

            // Choose the largest out of the current size and the calculated size based on the tank being to the left or right of the camera.
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / camera.aspect);
        }

        // Add the edge buffer to the size.
        size += SCREEN_EDGE_BUFFER;

        // Make sure the camera's size isn't below the minimum.
        size = Mathf.Max(size, MIN_SIZE);

        return size;
    }

    public void SetStartPositionAndSize()
    {
        // Find the desired position.
        FindAveragePosition();

        // Set the camera's position to the desired position without damping.
        this.instance.transform.position = desiredPosition + new Vector3(0.0f, yOffset, zOffset);

        // Find and set the required size of the camera.
        camera.orthographicSize = FindRequiredSize();
    }
}
