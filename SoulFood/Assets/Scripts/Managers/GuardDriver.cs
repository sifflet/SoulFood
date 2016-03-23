using UnityEngine;
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

    public GuardDriver(GameObject instance, GameObject cameraInstance, Transform spawnPoint)
        : base(instance, cameraInstance, spawnPoint)
    {
        this.isLeader = false;

        this.instance.GetComponent<NPCMovement>().MaxSpeed = MAX_SPEED;
        this.instance.AddComponent<GuardKeyboardInputs>();
        this.keyboardInputs = this.instance.GetComponent<GuardKeyboardInputs>();
        this.keyboardInputs.Setup(this);

        this.cameraDriver = new GuardsCameraDriver(cameraInstance);

        this.keyboardInputs.enabled = false;
        this.cameraDriver.SetEnabled(false);
        this.stateMachine = new GuardStateMachine(this);
    }

    public override void SetControlledByAI(bool controlledByAI)
    {
        this.controlledByAI = controlledByAI;
        this.keyboardInputs.enabled = !controlledByAI;
        if (IsLeader) this.cameraDriver.SetEnabled(!controlledByAI);
    }

    protected override void PassTime()
    {
        //decrease lunge timer here
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
