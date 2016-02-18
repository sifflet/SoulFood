using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {

	float timer = 2f; //Amount of time needed on the pad
	bool isTriggered = false;

	// Update is called once per frame
	void FixedUpdate () {
		if (isTriggered) {
			if (getButtonStatus ())
				timer = 0f;
			else
				timer -= Time.deltaTime; //timer decreased overtime
		} else {
			if(timer >= 2f)
				timer = 2f;
			else
				timer += Time.deltaTime; //timer recharged slowly
		}
	}

	public bool getButtonStatus(){
		if (timer <= 0f)
			return true;
		else
			return false;
	}

	void OnTriggerStay(Collider col)
	{
		if(col.gameObject.tag == "Player")//Change to whatever we name their tag
			isTriggered = true;
	}

	void OnTriggerExit(Collider col)
	{
		if(col.gameObject.tag == "Player")//Change to whatever we name their tag
			isTriggered = false;
	}
}
