using UnityEngine;
using System.Collections;

public class GuardLungeState : NPCState
{
    public GuardLungeState(NPCStateMachine stateMachine)
        : base(stateMachine)
    {
    }
	
	private float lungeTimer = 0f;		// time on lunging
	private float lungeDuration = 0.1f;	// lunge during X sec only

    public override void Entry()
    {
		Debug.Log (this.stateMachine.NPC.name + ": Lunge entry");

		lungeTimer = Time.time;
    }

    public override NPCState Update()
    {
		if(Time.time - lungeTimer <= lungeDuration) {
			NPCActions.Lunge(this.stateMachine.NPC.Instance);
		}
		
        return this;
    }

}
