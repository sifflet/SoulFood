using UnityEngine;
using System.Collections;

public abstract class CameraDriver
{
    protected GameObject instance;

    private const float yOffset = 5.0f;
    private const float zOffset = -5.0f;

    public CameraDriver(GameObject cameraInstance)
    {
        this.instance = cameraInstance;
    }

    public void SetEnabled(bool enabled)
    {
        this.instance.GetComponent<Camera>().enabled = enabled;
    }

    public abstract void Update();
}
