using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuardDriver : NPCDriver
{
    private bool isLeader;

    public bool IsLeader {
        get { return this.isLeader; } 
        set { this.isLeader = value; (this.keyboardInputs as GuardKeyboardInputs).IsLeader = true; }
    }

    public GuardDriver(GameObject instance, GameObject cameraInstance, Transform spawnPoint)
        : base(instance, cameraInstance, spawnPoint)
    {
        this.isLeader = false;
        this.movementDriver = new NPCMovementDriver(this.instance.GetComponent<NPCMovement>());

        this.instance.AddComponent<GuardKeyboardInputs>();
        this.keyboardInputs = this.instance.GetComponent<GuardKeyboardInputs>();

        this.cameraDriver = new GuardsCameraDriver(cameraInstance);

        this.keyboardInputs.enabled = false;
        this.cameraDriver.SetEnabled(false);
    }
}
