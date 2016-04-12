using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class SoulTree : NetworkBehaviour {

	private int triggeredCount;		// How many buttons are being stepped on by players
	[SyncVar (hook = "stopGhosts")] private bool isFull = true;
	private int treeType;
	private Transform buttons;
	private List<GameObject> treeButtons = new List<GameObject>();

	public int TreeType { get { return this.treeType; } }
	public bool IsFull { get { return this.isFull; } }
	public List<GameObject> TreeButtons { get { return this.treeButtons; } }
	public GameObject soulObject;

	void Awake () {
		buttons = transform.Find("Buttons");
		treeType = buttons.childCount;
		foreach (Transform child in buttons) 
		{
			Button childButton = child.GetComponent<Button>();
			if (childButton)	// Child is a button
			{
				treeButtons.Add(childButton.transform.gameObject);
			}
		}
	}

	void Update () {
		triggeredCount = 0;
		foreach (Transform button in buttons) {
			if(button.gameObject.GetComponent<Button>().GetButtonStatus())
				triggeredCount++;
		}
		if (triggeredCount++ == buttons.childCount) {
			DropItems();
		}
	}

	public bool CheckIfTreeIsTargetted()
	{
		foreach (GameObject buttonObj in this.treeButtons) {
			Button buttonScript = buttonObj.GetComponent<Button>();
			if (buttonScript.IsTargettedForTriggering)
				return true;
		}

		return false;
	}

	public bool CheckIfTreeIsCompletelyTargettedOrTriggered()
	{
		foreach (GameObject buttonObj in this.treeButtons) {
			Button buttonScript = buttonObj.GetComponent<Button>();
			if (!buttonScript.IsTargettedForTriggering && !buttonScript.IsTriggered)
				return false;
		}
		
		return true;
	}

	public bool CheckIfTreeHasMultipleButtons()
	{
		if (this.treeButtons.Count == 2 || this.treeButtons.Count == 3)
			return true;

		return false;
	}

	void SpawnSouls() {
		Vector3 explosionVec = transform.forward + transform.up; // minus because the prefab is kinda inverted... // new Vector3(transform.position.x, transform.position.y + 2.5f, transform.position.z + 2f);
		float explosionMagnitude = 100f;
		
		// Middle soul
		Vector3 offset = transform.forward + (Vector3.up*2);
		GameObject soul1 = (GameObject) Instantiate(soulObject, transform.position + offset, transform.rotation);
		soul1.GetComponent<Rigidbody>().AddForce((explosionVec + offset) * explosionMagnitude, ForceMode.Acceleration);
		NetworkServer.Spawn(soul1);
		
		// Right soul
		offset = transform.right + (Vector3.up*2);
		GameObject soul2 = (GameObject) Instantiate(soulObject, transform.position + offset, transform.rotation);
		soul2.GetComponent<Rigidbody>().AddForce((explosionVec + offset) * explosionMagnitude, ForceMode.Acceleration);
		NetworkServer.Spawn(soul2);
		
		// Left soul
		offset = -transform.right + (Vector3.up*2);
		GameObject soul3 = (GameObject) Instantiate(soulObject, transform.position + offset, transform.rotation);
		soul3.GetComponent<Rigidbody>().AddForce((explosionVec + offset) * explosionMagnitude, ForceMode.Acceleration);
		NetworkServer.Spawn(soul3);
	}
	
	void DropItems() {
		if (isFull) {
			
			print("treeButtons.Count: " + treeButtons.Count);
			
			for(int i = 0; i < treeButtons.Count; ++i) {
				Invoke("SpawnSouls", i);
			}
			
			isFull = false;
			
			this.GetComponentInChildren<ParticleSystem>().enableEmission = false;
			
			//Invoke("ResetTree", 60);//time in seconds
		}
	}

    void stopGhosts(bool status)
    {
        transform.Find("Tomb").GetComponent<ParticleSystem>().loop = status;
    }

    void ResetTree()
    {
        isFull = true;
        //Animator things here to show its full again
    }
}
