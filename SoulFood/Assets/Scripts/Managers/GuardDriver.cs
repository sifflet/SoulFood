using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuardDriver : NPCDriver
{
    public GuardDriver(GameObject instance, GameObject cameraInstance, Transform spawnPoint)
        : base(instance, cameraInstance, spawnPoint)
    {
        this.movementDriver = new NPCMovementDriver(this.instance.GetComponent<NPCMovement>());
        this.keyboardMovement = instance.GetComponent<Player>();

        List<GameObject> guards = new List<GameObject>();

        foreach (NPCDriver guard in GameManager.Guards)
        {
            guards.Add(guard.Instance);
        }

        this.cameraDriver = new GuardsCameraDriver(cameraInstance, guards);

        this.keyboardMovement.enabled = false;
        this.cameraDriver.SetEnabled(false);
    }
}
