using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class CollectorDriver : NPCDriver
{
    private const float MAX_SPEED = 15f;
    private int soulsStored = 0;
    private float immortalTimer = 0f;
    private bool isImmortal;

    private Material transparentMaterial = Resources.Load("Transparent", typeof(Material)) as Material;
    private Material[] knightMaterials = new Material[2];
    private Material swordMaterial;

	public bool isDroppingSoul = false;			// Used to trigger indicator sound
	public bool isCollectingSoul = false;		// Used to trigger indicator sound

    public int SoulsStored { get { return this.soulsStored; } }
    public float MaxSpeed { get { return MAX_SPEED; } }

    public bool IsImmortal
    {
        get { return this.isImmortal; }
        set
        {
            this.isImmortal = value;

            if (isImmortal)
            {
                immortalTimer = CollectorStateMachine.IMMORTALITY_TIME;
                this.KeyBoardInputs.speed += 3f;
                SetCollectorOpacity();
            }
        }
    }

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

        this.isImmortal = false;
    }

    public override void Update()
    {
        base.Update();

        if (isImmortal)
        {
            immortalTimer -= Time.deltaTime;

            if (immortalTimer <= 0)
            {
                RemoveCollectorOpacity();
                isImmortal = false;
                this.KeyBoardInputs.speed -= 3f;
            }
        }

		// Sound effects
		if (this.isDroppingSoul)
		{
			AudioClip popClip = Resources.Load("pop_sound", typeof(AudioClip)) as AudioClip;
			this.audioSource.clip = popClip;
			this.audioSource.Play();
			this.isDroppingSoul = false;
		}
		if (this.isCollectingSoul)
		{
			AudioClip pickupClip = Resources.Load("pickup_sound", typeof(AudioClip)) as AudioClip;
			this.audioSource.clip = pickupClip;
			this.audioSource.Play();
			this.isCollectingSoul = false;
		}
    }

    public void AddSoul()
    {
        ++this.soulsStored;
    }

    public void DropSoul(int soulsDropped)
    {
        if (this.soulsStored > 0)
        {
            this.soulsStored -= soulsDropped;
            InstantiateSouls(soulsDropped);
        }
    }

    private void InstantiateSouls(int numberOfSouls)
    {
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

    private void SetCollectorOpacity()
    {
        Transform knightTransform = this.Instance.transform.GetChild(0).GetChild(0);
        knightMaterials = knightTransform.gameObject.GetComponent<Renderer>().materials;
        Material[] transparentMaterials = new Material[2];
        transparentMaterials[0] = transparentMaterial;
        transparentMaterials[1] = transparentMaterial;
        knightTransform.gameObject.GetComponent<Renderer>().materials = transparentMaterials;

        Transform swordTransform = this.Instance.transform.GetChild(0).GetChild(4);
        swordMaterial = swordTransform.gameObject.GetComponent<Renderer>().material;
        swordTransform.gameObject.GetComponent<Renderer>().material = transparentMaterial;
    }

    private void RemoveCollectorOpacity()
    {
        Transform knightTransform = this.Instance.transform.GetChild(0).GetChild(0);
        knightTransform.gameObject.GetComponent<Renderer>().materials = knightMaterials;

        Transform swordTransform = this.Instance.transform.GetChild(0).GetChild(4);
        swordTransform.gameObject.GetComponent<Renderer>().material = swordMaterial;
    }
}