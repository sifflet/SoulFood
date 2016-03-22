using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectorDriver : NPCDriver
{
    private float eatingDelay = 0.5f;
    private const float MAX_SPEED = 15f;
    private int soulsStored = 0;

    public CollectorDriver(GameObject instance, GameObject cameraInstance, Transform spawnPoint)
        : base(instance, cameraInstance, spawnPoint)
    {
        this.instance.GetComponent<NPCMovement>().MaxSpeed = MAX_SPEED;
        this.instance.AddComponent<CollectorKeyboardInputs>();
        this.keyboardInputs = this.instance.GetComponent<CollectorKeyboardInputs>();

        this.cameraDriver = new CollectorsCameraDriver(cameraInstance, instance);

        this.keyboardInputs.enabled = false;
        this.cameraDriver.SetEnabled(false);

        this.stateMachine = new CollectorStateMachine(this);
    }

    protected override void FindVisibleNPCs()
    {
        this.visibleNPCs.Clear();

        List<NPCDriver> allNPCs = new List<NPCDriver>(GameManager.Deathies);
        allNPCs.AddRange(GameManager.Guards);

        foreach (NPCDriver npc in allNPCs)
        {
            if (npc == this) continue;

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