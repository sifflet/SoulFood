using UnityEngine;
using System.Collections;

public class GuardsCommonController : MonoBehaviour {

	private Vector3 movement;
	private float speed = 10.0f;
	private float angularSpeed = 180.0f;	// degrees per second
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public virtual void MoveGuard(float h, float v) {
		Debug.Log ("MoveGuard() from GuardsController");
		movement.x = h;
		movement.z = v;

		// move
		this.GetComponent<Rigidbody>().velocity = movement * speed;

		// rotate
		float step = speed * Time.deltaTime;
		Vector3 direction = movement.normalized;
		Vector3 rotation = Vector3.RotateTowards(transform.forward, direction, step, 0.0f);
		Debug.DrawRay(transform.position, rotation, Color.red);
		transform.rotation = Quaternion.LookRotation(rotation);
	
	}
	
}
