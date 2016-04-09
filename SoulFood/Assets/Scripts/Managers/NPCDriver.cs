using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;

[Serializable]
public abstract class NPCDriver : NetworkBehaviour
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
	public CameraDriver CameraDriver { get { return this.cameraDriver; } }
    public NPCStateMachine StateMachine { get { return this.stateMachine; } }
	public bool ControlledByAI { get { return this.controlledByAI; } }

    public override void OnStartServer()
    {
        cameraDriver.SetEnabled(true);
    }
    
    public Collider[] CollisionArray
    {
        get
        {
            Vector3 location = this.instance.transform.position;
            location.y = 0.0f;
            return Physics.OverlapSphere(location, GameManager.COLLISION_RANGE);
        }
    }
    
    public virtual void Update()
    {
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

    public void Sacrebleu()
    {
        this.movementDriver.Setup(this.GetComponent<NPCMovement>());
    }

    protected abstract void FindVisibleNPCs();
}