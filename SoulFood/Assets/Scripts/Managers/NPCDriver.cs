using UnityEngine;
using System.Collections;
using System;

[Serializable]
public abstract class NPCDriver
{
    protected GameObject instance;
    protected bool controlledByAI;
    protected Transform spawnPoint;

    protected NPCMovementDriver movementDriver;
    protected Player keyboardMovement;
    protected CameraDriver cameraDriver;

    public GameObject Instance { get { return this.instance; } }

    protected NPCDriver(GameObject instance, GameObject cameraInstance, Transform spawnPoint)
    {
        this.instance = instance;
        this.controlledByAI = true;
        this.spawnPoint = spawnPoint;
    }

    public void SetControlledByAI(bool controlledByAI)
    {
        this.controlledByAI = controlledByAI;
        this.keyboardMovement.enabled = !controlledByAI;
        this.cameraDriver.SetEnabled(!controlledByAI);
    }

    public void Update()
    {
        if (controlledByAI)
        {
            movementDriver.Update();
        }
        else
        {
            cameraDriver.Update();
        }
    }
}