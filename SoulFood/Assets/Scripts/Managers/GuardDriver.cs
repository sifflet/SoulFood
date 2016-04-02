﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuardDriver : NPCDriver
{
    private bool isLeader;
    private const float MAX_SPEED = 12f;

    public bool IsLeader {
        get { return this.isLeader; } 
        set { this.isLeader = value; (this.keyboardInputs as GuardKeyboardInputs).IsLeader = true; }
    }

    public override void Setup(GameObject instance, GameObject cameraInstance, Transform spawnPoint)
    {
        base.Setup(instance, cameraInstance, spawnPoint);

        this.isLeader = false;

        this.instance.GetComponent<NPCMovement>().MaxSpeed = MAX_SPEED;
        this.instance.AddComponent<GuardKeyboardInputs>();
        this.keyboardInputs = this.instance.GetComponent<GuardKeyboardInputs>();
        this.keyboardInputs.Setup(this);

        this.instance.AddComponent<GuardsCameraDriver>();
        this.cameraDriver = this.instance.GetComponent<GuardsCameraDriver>();
        this.cameraDriver.Setup(cameraInstance);

        this.keyboardInputs.enabled = false;
        this.cameraDriver.SetEnabled(false);

        this.instance.AddComponent<GuardStateMachine>();
        this.stateMachine = this.instance.GetComponent<GuardStateMachine>();
        this.stateMachine.Setup(this);
    }

    public override void SetControlledByAI(bool controlledByAI)
    {
        this.controlledByAI = controlledByAI;
        this.keyboardInputs.enabled = !controlledByAI;
        if (IsLeader) this.cameraDriver.SetEnabled(!controlledByAI);
        this.stateMachine.enabled = controlledByAI;
        this.movementDriver.enabled = controlledByAI;
    }

    protected override void FindVisibleNPCs()
    {
        this.visibleNPCs.Clear();

        List<NPCDriver> allNPCs = new List<NPCDriver>(GameManager.Deathies);
        allNPCs.AddRange(GameManager.Guards);

        foreach (NPCDriver npc in allNPCs)
        {
            if (npc == this) continue;
            if (npc.GetType() == typeof(GuardDriver)) continue;

            Vector3 viewPortPosition = this.cameraDriver.Camera.WorldToViewportPoint(npc.Instance.transform.position);

            if (viewPortPosition.x >= 0.0f && viewPortPosition.x <= 1.0f &&
                viewPortPosition.y >= 0.0f && viewPortPosition.y <= 1.0f &&
                viewPortPosition.z >= 0.0f)
            {
                this.visibleNPCs.Add(npc);
            }
        }
    }
}
