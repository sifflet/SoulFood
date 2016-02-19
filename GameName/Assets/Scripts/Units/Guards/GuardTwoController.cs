using UnityEngine;
using System.Collections;

public class GuardTwoController : GuardsCommonController {
	
	void Update() {
		
		// Getting controls of Guard 2
		float h = Input.GetAxis("Horizontal2");
		float v = Input.GetAxis("Vertical2");
		
		MoveGuard(h, v);
		
	}
	
	public override void MoveGuard(float h, float v) {
		
		base.MoveGuard(h, v);
		
		Debug.Log("Guard #2: " + h + " - " + v);
		
	}
	
}
