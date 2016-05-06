using UnityEngine;
using System.Collections;

public abstract class CameraDriver : MonoBehaviour
{
    protected GameObject instance;
    protected Camera camera;

    private const float yOffset = 5.0f;
    private const float zOffset = -5.0f;

    public Camera Camera { get { return this.camera; } }

    public virtual void Setup(GameObject cameraInstance)
    {
        this.instance = cameraInstance;
        this.camera = instance.GetComponent<Camera>();
    }

    public virtual void Setup(GameObject cameraInstance, GameObject target)
    {
        this.instance = cameraInstance;
        this.camera = instance.GetComponent<Camera>();
    }

    public void SetEnabled(bool enabled)
    {
        this.instance.GetComponent<Camera>().enabled = enabled;
    }

    public virtual void Update() { }
}
