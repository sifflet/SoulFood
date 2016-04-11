using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class SoulTree : NetworkBehaviour {

	private int triggeredCount;		// How many buttons are being stepped on by players
	private bool isFull = true;
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

	void DropItems(){
		if (isFull) {
            GameObject soul1, soul2, soul3, soul4, soul5, soul6, soul7, soul8, soul9;
            Vector3 explosionVec = new Vector3(transform.position.x, transform.position.y + 2.5f, transform.position.z + 2f);
            float explosionMagnitude = 350f;
            switch (treeButtons.Count) //General Idea, to be tweaked
            {
                case 3:
                    soul9 = (GameObject) Instantiate(soulObject, new Vector3(transform.position.x, transform.position.y + 5, transform.position.z), Quaternion.identity);
                    soul8 = (GameObject) Instantiate(soulObject, new Vector3(transform.position.x + 2f, transform.position.y + 5, transform.position.z), Quaternion.identity);
                    soul7 = (GameObject) Instantiate(soulObject, new Vector3(transform.position.x - 2f, transform.position.y + 5, transform.position.z), Quaternion.identity);
                    soul6 = (GameObject) Instantiate(soulObject, new Vector3(transform.position.x, transform.position.y + 4, transform.position.z), Quaternion.identity);
                    soul5 = (GameObject) Instantiate(soulObject, new Vector3(transform.position.x + 2.5f, transform.position.y + 4, transform.position.z), Quaternion.identity);
                    soul4 = (GameObject) Instantiate(soulObject, new Vector3(transform.position.x - 2.5f, transform.position.y + 4, transform.position.z), Quaternion.identity);
                    soul3 = (GameObject) Instantiate(soulObject, new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), Quaternion.identity);
                    soul2 = (GameObject) Instantiate(soulObject, new Vector3(transform.position.x + 1.5f, transform.position.y + 3, transform.position.z), Quaternion.identity);
                    soul1 = (GameObject) Instantiate(soulObject, new Vector3(transform.position.x - 1.5f, transform.position.y + 3, transform.position.z), Quaternion.identity);
                    soul1.GetComponent<Rigidbody>().AddExplosionForce(explosionMagnitude, explosionVec, 5f);
                    soul2.GetComponent<Rigidbody>().AddExplosionForce(explosionMagnitude, explosionVec, 5f);
                    soul3.GetComponent<Rigidbody>().AddExplosionForce(explosionMagnitude, explosionVec, 5f);
                    soul4.GetComponent<Rigidbody>().AddExplosionForce(explosionMagnitude, explosionVec, 5f);
                    soul5.GetComponent<Rigidbody>().AddExplosionForce(explosionMagnitude, explosionVec, 5f);
                    soul6.GetComponent<Rigidbody>().AddExplosionForce(explosionMagnitude, explosionVec, 5f);
                    soul7.GetComponent<Rigidbody>().AddExplosionForce(explosionMagnitude, explosionVec, 5f);
                    soul8.GetComponent<Rigidbody>().AddExplosionForce(explosionMagnitude, explosionVec, 5f);
                    soul9.GetComponent<Rigidbody>().AddExplosionForce(explosionMagnitude, explosionVec, 5f);
                    NetworkServer.Spawn(soul1);
                    NetworkServer.Spawn(soul2);
                    NetworkServer.Spawn(soul3);
                    NetworkServer.Spawn(soul4);
                    NetworkServer.Spawn(soul5);
                    NetworkServer.Spawn(soul6);
                    NetworkServer.Spawn(soul7);
                    NetworkServer.Spawn(soul8);
                    NetworkServer.Spawn(soul9);
                    break;
                case 2:
                    soul6 = (GameObject) Instantiate(soulObject, new Vector3(transform.position.x, transform.position.y + 4, transform.position.z), Quaternion.identity);
                    soul5 = (GameObject) Instantiate(soulObject, new Vector3(transform.position.x + 1.5f, transform.position.y + 4, transform.position.z), Quaternion.identity);
                    soul4 = (GameObject) Instantiate(soulObject, new Vector3(transform.position.x - 1.5f, transform.position.y + 4, transform.position.z), Quaternion.identity);
                    soul3 = (GameObject) Instantiate(soulObject, new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), Quaternion.identity);
                    soul2 = (GameObject) Instantiate(soulObject, new Vector3(transform.position.x + 1.5f, transform.position.y + 3, transform.position.z), Quaternion.identity);
                    soul1 = (GameObject) Instantiate(soulObject, new Vector3(transform.position.x - 1.5f, transform.position.y + 3, transform.position.z), Quaternion.identity);
                    soul1.GetComponent<Rigidbody>().AddExplosionForce(explosionMagnitude, explosionVec, 5f);
                    soul2.GetComponent<Rigidbody>().AddExplosionForce(explosionMagnitude, explosionVec, 5f);
                    soul3.GetComponent<Rigidbody>().AddExplosionForce(explosionMagnitude, explosionVec, 5f);
                    soul4.GetComponent<Rigidbody>().AddExplosionForce(explosionMagnitude, explosionVec, 5f);
                    soul5.GetComponent<Rigidbody>().AddExplosionForce(explosionMagnitude, explosionVec, 5f);
                    soul6.GetComponent<Rigidbody>().AddExplosionForce(explosionMagnitude, explosionVec, 5f);
                    NetworkServer.Spawn(soul1);
                    NetworkServer.Spawn(soul2);
                    NetworkServer.Spawn(soul3);
                    NetworkServer.Spawn(soul4);
                    NetworkServer.Spawn(soul5);
                    NetworkServer.Spawn(soul6);
                    break;
                default:
                    soul3 = (GameObject) Instantiate(soulObject, new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), Quaternion.identity);
                    soul2 = (GameObject) Instantiate(soulObject, new Vector3(transform.position.x + 1.5f, transform.position.y + 3, transform.position.z), Quaternion.identity);
                    soul1 = (GameObject) Instantiate(soulObject, new Vector3(transform.position.x - 1.5f, transform.position.y + 3, transform.position.z), Quaternion.identity);
                    soul1.GetComponent<Rigidbody>().AddExplosionForce(explosionMagnitude, explosionVec, 5f);
                    soul2.GetComponent<Rigidbody>().AddExplosionForce(explosionMagnitude, explosionVec, 5f);
                    soul3.GetComponent<Rigidbody>().AddExplosionForce(explosionMagnitude, explosionVec, 5f);
                    NetworkServer.Spawn(soul1);
                    NetworkServer.Spawn(soul2);
                    NetworkServer.Spawn(soul3);
                    break;
            }
            isFull = false;
            //Invoke("ResetTree", 60);//time in seconds
		}
	}

    void ResetTree()
    {
        isFull = true;
        //Animator things here to show its full again
    }
}
