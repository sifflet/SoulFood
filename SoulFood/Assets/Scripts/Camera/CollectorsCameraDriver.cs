using UnityEngine;
using System.Collections;

public class CollectorsCameraDriver : CameraDriver
{
    private GameObject target;

    private const float yOffset = 5.0f;
    private const float zOffset = -5.0f;

    public CollectorsCameraDriver(GameObject cameraInstance, GameObject target)
        : base(cameraInstance)
    {
        this.target = target;
    }

    public override void Update()
    {
        Vector3 newPosition = target.transform.position;
        newPosition.z += zOffset;
        newPosition.y += yOffset;

        instance.transform.position = newPosition;
    }
}
