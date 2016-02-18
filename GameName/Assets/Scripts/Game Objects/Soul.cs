using UnityEngine;
using System.Collections;

public class Soul : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Player")//Change to whatever we name their tag
			col.SendMessage ("CanEat"); //need player to intergrate this
	}
}
