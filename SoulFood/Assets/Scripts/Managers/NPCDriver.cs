using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class NPCDriver
{
    private GameObject instance;
    private bool controlledByAI;
    private Transform spawnPoint;

    private NPCMovementDriver movementDriver;
    private Player keyboardMovement;

    public NPCDriver(GameObject instance, Transform spawnPoint, bool controlledByAI)
    {
        this.instance = instance;
        this.controlledByAI = controlledByAI;
        this.spawnPoint = spawnPoint;

        this.movementDriver = new NPCMovementDriver(this.instance.GetComponent<NPCMovement>());
        this.keyboardMovement = instance.GetComponent<Player>();

        if (controlledByAI) this.keyboardMovement.enabled = false;
    }

    public void Update()
    {
        if(controlledByAI) movementDriver.Update();
    }
}
