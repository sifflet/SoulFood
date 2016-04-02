using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public abstract class NPCDriver : MonoBehaviour
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
    public NPCStateMachine StateMachine { get { return this.stateMachine; } }

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
            //movementDriver.Update();
            //stateMachine.Update();
        }
        PassTime();
        //cameraDriver.Update();
        FindVisibleNPCs();
    }

    public virtual void Setup(GameObject instance, GameObject cameraInstance, Transform spawnPoint)
    {
        this.instance = instance;
        this.controlledByAI = true;
        this.spawnPoint = spawnPoint;

        this.visibleNPCs = new List<NPCDriver>();

        this.instance.AddComponent<NPCMovementDriver>();
        this.movementDriver = this.instance.GetComponent<NPCMovementDriver>();
        this.movementDriver.Setup(this.GetComponent<NPCMovement>());
    }

    public virtual void SetControlledByAI(bool controlledByAI)
    {
        this.controlledByAI = controlledByAI;
        this.keyboardInputs.enabled = !controlledByAI;
        this.cameraDriver.SetEnabled(!controlledByAI);
        this.stateMachine.enabled = controlledByAI;
        this.movementDriver.enabled = controlledByAI;
    }

    public void StartStateMachine()
    {
        this.stateMachine.EnterFirstState();
    }
    protected abstract void PassTime();
    protected abstract void FindVisibleNPCs();
}