﻿using UnityEngine;
using System.Collections;

public class Tree : MonoBehaviour {
	int triggeredCount;
	public GameObject souls;
	bool isFull = true;


	void Update () {
		triggeredCount = 0;
		foreach (Transform button in transform) {
			if(button.gameObject.GetComponent<Button>().GetButtonStatus())
				triggeredCount++;
		}
		if (triggeredCount++ == transform.childCount) {
			DropItems();
		}
	}

	void DropItems(){
		if (isFull) {
            GameObject soul1, soul2, soul3, soul4, soul5, soul6, soul7, soul8, soul9;
            Vector3 explosionVec = new Vector3(transform.position.x, transform.position.y + 2.5f, transform.position.z + 2f);
            float explosionMagnitude = 350f;
            switch (transform.childCount) //General Idea, to be tweaked
            {
                case 3:
                    soul9 = (GameObject) Instantiate(souls, new Vector3(transform.position.x, transform.position.y + 5, transform.position.z), Quaternion.identity);
                    soul8 = (GameObject) Instantiate(souls, new Vector3(transform.position.x + 2f, transform.position.y + 5, transform.position.z), Quaternion.identity);
                    soul7 = (GameObject) Instantiate(souls, new Vector3(transform.position.x - 2f, transform.position.y + 5, transform.position.z), Quaternion.identity);
                    soul6 = (GameObject) Instantiate(souls, new Vector3(transform.position.x, transform.position.y + 4, transform.position.z), Quaternion.identity);
                    soul5 = (GameObject) Instantiate(souls, new Vector3(transform.position.x + 2.5f, transform.position.y + 4, transform.position.z), Quaternion.identity);
                    soul4 = (GameObject) Instantiate(souls, new Vector3(transform.position.x - 2.5f, transform.position.y + 4, transform.position.z), Quaternion.identity);
                    soul3 = (GameObject) Instantiate(souls, new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), Quaternion.identity);
                    soul2 = (GameObject) Instantiate(souls, new Vector3(transform.position.x + 1.5f, transform.position.y + 3, transform.position.z), Quaternion.identity);
                    soul1 = (GameObject) Instantiate(souls, new Vector3(transform.position.x - 1.5f, transform.position.y + 3, transform.position.z), Quaternion.identity);
                    soul1.GetComponent<Rigidbody>().AddExplosionForce(explosionMagnitude, explosionVec, 5f);
                    soul2.GetComponent<Rigidbody>().AddExplosionForce(explosionMagnitude, explosionVec, 5f);
                    soul3.GetComponent<Rigidbody>().AddExplosionForce(explosionMagnitude, explosionVec, 5f);
                    soul4.GetComponent<Rigidbody>().AddExplosionForce(explosionMagnitude, explosionVec, 5f);
                    soul5.GetComponent<Rigidbody>().AddExplosionForce(explosionMagnitude, explosionVec, 5f);
                    soul6.GetComponent<Rigidbody>().AddExplosionForce(explosionMagnitude, explosionVec, 5f);
                    soul7.GetComponent<Rigidbody>().AddExplosionForce(explosionMagnitude, explosionVec, 5f);
                    soul8.GetComponent<Rigidbody>().AddExplosionForce(explosionMagnitude, explosionVec, 5f);
                    soul9.GetComponent<Rigidbody>().AddExplosionForce(explosionMagnitude, explosionVec, 5f);
                    break;
                case 2:
                    soul6 = (GameObject) Instantiate(souls, new Vector3(transform.position.x, transform.position.y + 4, transform.position.z), Quaternion.identity);
                    soul5 = (GameObject) Instantiate(souls, new Vector3(transform.position.x + 1.5f, transform.position.y + 4, transform.position.z), Quaternion.identity);
                    soul4 = (GameObject) Instantiate(souls, new Vector3(transform.position.x - 1.5f, transform.position.y + 4, transform.position.z), Quaternion.identity);
                    soul3 = (GameObject) Instantiate(souls, new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), Quaternion.identity);
                    soul2 = (GameObject) Instantiate(souls, new Vector3(transform.position.x + 1.5f, transform.position.y + 3, transform.position.z), Quaternion.identity);
                    soul1 = (GameObject) Instantiate(souls, new Vector3(transform.position.x - 1.5f, transform.position.y + 3, transform.position.z), Quaternion.identity);
                    soul1.GetComponent<Rigidbody>().AddExplosionForce(explosionMagnitude, explosionVec, 5f);
                    soul2.GetComponent<Rigidbody>().AddExplosionForce(explosionMagnitude, explosionVec, 5f);
                    soul3.GetComponent<Rigidbody>().AddExplosionForce(explosionMagnitude, explosionVec, 5f);
                    soul4.GetComponent<Rigidbody>().AddExplosionForce(explosionMagnitude, explosionVec, 5f);
                    soul5.GetComponent<Rigidbody>().AddExplosionForce(explosionMagnitude, explosionVec, 5f);
                    soul6.GetComponent<Rigidbody>().AddExplosionForce(explosionMagnitude, explosionVec, 5f);
                    break;
                default:
                    soul3 = (GameObject) Instantiate(souls, new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), Quaternion.identity);
                    soul2 = (GameObject) Instantiate(souls, new Vector3(transform.position.x + 1.5f, transform.position.y + 3, transform.position.z), Quaternion.identity);
                    soul1 = (GameObject) Instantiate(souls, new Vector3(transform.position.x - 1.5f, transform.position.y + 3, transform.position.z), Quaternion.identity);
                    soul1.GetComponent<Rigidbody>().AddExplosionForce(explosionMagnitude, explosionVec, 5f);
                    soul2.GetComponent<Rigidbody>().AddExplosionForce(explosionMagnitude, explosionVec, 5f);
                    soul3.GetComponent<Rigidbody>().AddExplosionForce(explosionMagnitude, explosionVec, 5f);
                    break;
            }
            isFull = false;
            Invoke("resetTree", 60);//time in seconds
		}
	}

    void ResetTree()
    {
        isFull = true;
        //Animator things here to show its full again
    }
}
