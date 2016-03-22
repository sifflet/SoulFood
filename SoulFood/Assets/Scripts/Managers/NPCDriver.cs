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

    protected NPCStateMachine stateMachine;

    public GameObject Instance { get { return this.instance; } }
    public List<NPCDriver> VisibleNPCs { get { return this.visibleNPCs; } }
    public NPCMovementDriver MovementDriver { get { return this.movementDriver; } }

    public Collider[] CollisionArray
    {
        get
        {
            Vector3 location = this.instance.GetComponent<NPCMovement>().transform.position;
            return Physics.OverlapSphere(location, 2.0f);
        }
    }

    public void Update()
    {
        if (controlledByAI)
        {
            movementDriver.Update();
            stateMachine.Update();
        }

        cameraDriver.Update();
        FindVisibleNPCs();
    }

    protected NPCDriver(GameObject instance, GameObject cameraInstance, Transform spawnPoint)
    {
        this.instance = instance;
        this.controlledByAI = true;
        this.spawnPoint = spawnPoint;

        this.visibleNPCs = new List<NPCDriver>();

        this.movementDriver = new NPCMovementDriver(this.instance.GetComponent<NPCMovement>());
    }

    public virtual void SetControlledByAI(bool controlledByAI)
    {
        this.controlledByAI = controlledByAI;
        this.keyboardInputs.enabled = !controlledByAI;
        this.cameraDriver.SetEnabled(!controlledByAI);
    }

    public void SetupStateMachine()
    {
        this.stateMachine.Setup();
    }

    protected abstract void FindVisibleNPCs();
}