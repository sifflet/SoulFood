using UnityEngine;
using System.Collections;

public class CameraDriver
{
    private GameObject instance;
    private GameObject target;

    private const float yOffset = 5.0f;
    private const float zOffset = -5.0f;

    public CameraDriver(GameObject cameraInstance, GameObject target)
    {
        this.instance = cameraInstance;
        this.target = target;
    }

    public void SetEnabled(bool enabled)
    {
        this.instance.GetComponent<Camera>().enabled = enabled;
    }

    public void Update()
    {
        Vector3 newPosition = target.transform.position;
        newPosition.z += zOffset;
        newPosition.y += yOffset;

        instance.transform.position = newPosition;
    }
}
