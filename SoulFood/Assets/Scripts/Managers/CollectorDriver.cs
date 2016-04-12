using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class CollectorDriver : NPCDriver
{
    private const float MAX_SPEED = 15f;
    private int soulsStored = 0;

    public int SoulsStored { get { return this.soulsStored; } }
	public float MaxSpeed { get { return MAX_SPEED; } }

    public override void Setup(GameObject instance, GameObject cameraInstance, Transform spawnPoint, GameObject soulPrefab)
    {
		base.Setup(instance, cameraInstance, spawnPoint, soulPrefab);

		this.instance.GetComponent<NPCMovement>().MaxSpeed = MAX_SPEED;

        this.instance.AddComponent<CollectorKeyboardInputs>();
        this.keyboardInputs = this.instance.GetComponent<CollectorKeyboardInputs>();
        this.keyboardInputs.Setup(this);

        this.instance.AddComponent<CollectorsCameraDriver>();
        this.cameraDriver = this.instance.GetComponent<CollectorsCameraDriver>();
        this.cameraDriver.Setup(cameraInstance, this.instance);

        this.keyboardInputs.enabled = false;
        this.cameraDriver.SetEnabled(false);

        this.instance.AddComponent<CollectorStateMachine>();
        this.stateMachine = this.instance.GetComponent<CollectorStateMachine>();
        this.stateMachine.Setup(this);
    }

    public override void Update()
    {
        base.Update();
    }


    /*
     *  Acquire soul prefab from GameManager since cannot set the soul prefab through the inspector on this script
     */
    public void SetSoulPrefab(GameObject soulPrefab)
    {
        this.soulPrefab = soulPrefab;
    }

    public void AddSoul()
    {
        ++this.soulsStored;
    }

    public void DropSoul(int soulsDropped)
    {
		Debug.Log (this.name + ": Dropped a soul!");
        if (this.soulsStored > 0)
        {
            this.soulsStored -= soulsDropped;
            InstantiateSouls(soulsDropped);
        }
    }

    private void InstantiateSouls(int numberOfSouls)
    {
		Debug.Log (this.name + ": Instantiated a soul!");
        Vector3 explosionVector = new Vector3(transform.position.x, transform.position.y + 2.5f, transform.position.z + 2f);
        float explosionMagnitude = 200f;
        float explosionRadius = 2f;

        for (int i = 0; i < numberOfSouls; ++i)
        {
            GameObject soul = (GameObject)Instantiate(this.soulPrefab, new Vector3(transform.position.x, transform.position.y + 5, transform.position.z), Quaternion.identity);
            soul.GetComponent<Rigidbody>().AddExplosionForce(explosionMagnitude, explosionVector, explosionRadius);
			NetworkServer.Spawn(soul);
        }
    }

    protected override void FindVisibleNPCs()
    {
        this.visibleNPCs.Clear();

        foreach (NPCDriver npc in GameManager.AllNPCs)
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