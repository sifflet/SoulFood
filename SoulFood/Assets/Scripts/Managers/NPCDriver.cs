using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class NPCDriver
{
    private GameObject instance;
    private Transform spawnPoint;

    private NPCMovementDriver movementDriver;

    public NPCDriver(GameObject instance, Transform spawnPoint)
    {
        this.instance = instance;
        this.spawnPoint = spawnPoint;

        this.movementDriver = new NPCMovementDriver(this.instance.GetComponent<NPCMovement>());
    }

    public void Update()
    {
        movementDriver.Update();
    }
}
