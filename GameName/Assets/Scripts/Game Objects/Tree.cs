using UnityEngine;
using System.Collections;

public class Tree : MonoBehaviour {
	int triggeredCount;
	public GameObject souls; //Attach soul prefab here


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
		//Dump whatever items you need
	}
}
