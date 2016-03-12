using UnityEngine;
using System.Collections;

public class CollectorDriver : NPCDriver
{
    public CollectorDriver(GameObject instance, GameObject cameraInstance, Transform spawnPoint)
        : base(instance, cameraInstance, spawnPoint)
    {
        this.movementDriver = new NPCMovementDriver(this.instance.GetComponent<NPCMovement>());
        this.keyboardMovement = instance.GetComponent<Player>();
        this.cameraDriver = new CameraDriver(cameraInstance, instance);

        this.keyboardMovement.enabled = false;
        this.cameraDriver.SetEnabled(false);
    }

}
