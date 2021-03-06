﻿using UnityEngine;
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
	protected GameObject soulPrefab;

    protected CameraDriver cameraDriver;
    protected List<NPCDriver> visibleNPCs;

    protected NPCStateMachine stateMachine;

	// Vars for sound effects
	public AudioSource audioSource;
	
    public GameObject Instance { get { return this.instance; } }
    public List<NPCDriver> VisibleNPCs { get { return this.visibleNPCs; } }
    public NPCMovementDriver MovementDriver { get { return this.movementDriver; } }
	public CameraDriver CameraDriver { get { return this.cameraDriver; } }
    public NPCStateMachine StateMachine { get { return this.stateMachine; } }
	public bool ControlledByAI { get { return this.controlledByAI; } }
    public KeyboardInputs KeyBoardInputs { get { return this.keyboardInputs; } }
    
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
        if(controlledByAI) FindVisibleNPCs();
    }

    public virtual void Setup(GameObject instance, GameObject cameraInstance, Transform spawnPoint, GameObject soulPrefab)
    {
        this.instance = instance;
        this.controlledByAI = true;
        this.spawnPoint = spawnPoint;
		this.soulPrefab = soulPrefab;

        this.visibleNPCs = new List<NPCDriver>();

        this.instance.AddComponent<NPCMovementDriver>();
        this.movementDriver = this.instance.GetComponent<NPCMovementDriver>();
        this.movementDriver.Setup(this.GetComponent<NPCMovement>());

		this.audioSource = this.instance.GetComponent<AudioSource>();
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