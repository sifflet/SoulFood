using UnityEngine;
using System.Collections;

public class CollectorDriver : NPCDriver
{
    public CollectorDriver(GameObject instance, GameObject cameraInstance, Transform spawnPoint)
        : base(instance, cameraInstance, spawnPoint)
    {
        this.movementDriver = new NPCMovementDriver(this.instance.GetComponent<NPCMovement>());

        this.instance.AddComponent<CollectorKeyboardInputs>();
        this.keyboardInputs = this.instance.GetComponent<CollectorKeyboardInputs>();

        this.cameraDriver = new CollectorsCameraDriver(cameraInstance, instance);

        this.keyboardInputs.enabled = false;
        this.cameraDriver.SetEnabled(false);
    }

}
