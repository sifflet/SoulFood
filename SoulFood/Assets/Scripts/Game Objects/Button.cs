using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {

	float timer = 2f; //Amount of time needed on the pad
	bool isTriggered = false;

	public bool IsTriggered { get { return this.isTriggered; } }

	// Update is called once per frame
	void FixedUpdate () {
		if (isTriggered) {
			if (GetButtonStatus ())
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

	public bool GetButtonStatus(){
		if (timer <= 0f)
			return true;
		else
			return false;
	}

	public SoulTree GetSoulTreeForCurrentButton() 
	{
		return this.transform.root.GetComponent<SoulTree>();
	}

	void OnTriggerStay(Collider col)
	{
		CollectorDriver collectorDriver = col.GetComponent<CollectorDriver>();
		if(collectorDriver)
			isTriggered = true;
	}

	void OnTriggerExit(Collider col)
	{
		CollectorDriver collectorDriver = col.GetComponent<CollectorDriver>();
		if(collectorDriver)
			isTriggered = false;
	}
}
