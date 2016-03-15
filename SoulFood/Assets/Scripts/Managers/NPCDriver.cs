using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public abstract class NPCDriver
{
    protected GameObject instance;
    protected bool controlledByAI;
    protected Transform spawnPoint;

    protected NPCMovementDriver movementDriver;
    protected KeyboardInputs keyboardInputs;
    protected CameraDriver cameraDriver;

    protected List<NPCDriver> visibleNPCs;

    public GameObject Instance { get { return this.instance; } }
    public List<NPCDriver> VisibleNPCs { get { return this.visibleNPCs; } }

    protected NPCDriver(GameObject instance, GameObject cameraInstance, Transform spawnPoint)
    {
        this.instance = instance;
        this.controlledByAI = true;
        this.spawnPoint = spawnPoint;

        this.visibleNPCs = new List<NPCDriver>();
    }

    public virtual void SetControlledByAI(bool controlledByAI)
    {
        this.controlledByAI = controlledByAI;
        this.keyboardInputs.enabled = !controlledByAI;
        this.cameraDriver.SetEnabled(!controlledByAI);
    }

    public void Update()
    {
        if(controlledByAI) movementDriver.Update();
        cameraDriver.Update();
        FindVisibleNPCs();
    }

    protected virtual void FindVisibleNPCs() { }
}