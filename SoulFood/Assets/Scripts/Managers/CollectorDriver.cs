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

    protected override void HandleCollisions() {
        eatingDelay -= Time.deltaTime;
        Vector3 location = this.instance.GetComponent<NPCMovement>().transform.position;
        Soul closestSoul = null;
        float closestDistance = 2f; //adjust size upon implementation
        Collider[] collisionArray = Physics.OverlapSphere(location, 2.0f);
        for (int i = 0; i < collisionArray.Length; i++)
        {
            if (collisionArray[i].tag == "Soul" && Mathf.Abs((location - collisionArray[i].transform.position).magnitude) <= closestDistance)
            {
                closestSoul = collisionArray[i].GetComponent<Soul>();
            }

            if (collisionArray[i].tag == "Guard")
            {
                HandleGuardCollision();
            }
        }
        if(closestSoul != null && eatingDelay <= 0f && Input.GetKeyDown("space"))
        {
            eatSoul(closestSoul);
        };
    }

    private void HandleGuardCollision()
    {
        GameManager.SoulEjected(soulsStored);
        soulsStored = 0;
        //Guard collision logic here
        //~~Immunity, speed, creation of souls to drop
    }

    private void eatSoul(Soul targetSoul)
    {
        eatingDelay = 0.5f;
        soulsStored++;
        targetSoul.SendMessage("getConsumed");
        GameManager.SoulConsumed();
        //Add slowing of speed
    }
}