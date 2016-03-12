using UnityEngine;
using System.Collections;

public class Tree : MonoBehaviour {
	int triggeredCount;
	public GameObject souls;
	bool isFull = true;


	void Update () {
		triggeredCount = 0;
		foreach (Transform button in transform) {
			if(button.gameObject.GetComponent<Button>().getButtonStatus())
				triggeredCount++;
		}
		if (triggeredCount++ == transform.childCount) {
			dropItems();
		}
	}

	void dropItems(){
		if (isFull) {
			Instantiate(souls,new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), Quaternion.identity);
			isFull = false;
		}
	}
}
