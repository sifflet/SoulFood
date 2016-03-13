using UnityEngine;
using System.Collections;

public class GuardOneController : GuardsCommonController {
	
	void Update() {
		
		// Getting controls of Guard 1
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");
		
		MoveGuard(h, v);
		
	}
	
	public override void MoveGuard(float h, float v) {
		
		base.MoveGuard(h, v);

		// custom behaviour for Guard One here
		
	}
	
}
