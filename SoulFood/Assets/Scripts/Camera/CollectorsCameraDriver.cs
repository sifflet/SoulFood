using UnityEngine;
using System.Collections;

public class CollectorsCameraDriver : CameraDriver
{
    private GameObject target;

    private const float yOffset = 8.0f;
    private const float zOffset = -8.0f;

    public override void Setup(GameObject cameraInstance, GameObject target)
    {
        base.Setup(cameraInstance);
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
