using UnityEngine;
using System.Collections;

public class GuardsCommonController : MonoBehaviour {
	
	private float speed = 5.0f;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public virtual void MoveGuard(float h, float v) {
		Debug.Log ("MoveGuard() from GuardsController");
		Vector3 direction = new Vector3(h, 0, v);
		
		if(v != 0)
			this.transform.Translate(Vector3.forward * v * speed * Time.deltaTime);
		
		// Rotate the Vehicle
		if(h != 0)
			transform.rotation = Quaternion.Lerp(transform.rotation,  Quaternion.LookRotation(direction)*transform.rotation, Time.deltaTime);
	}
	
}
