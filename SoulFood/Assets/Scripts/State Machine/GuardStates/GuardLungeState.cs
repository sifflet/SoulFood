using UnityEngine;
using System.Collections;

public class GuardLungeState : NPCState
{
    public GuardLungeState(NPCStateMachine stateMachine)
        : base(stateMachine)
    {
    }
	
	private float lungeTimer = 0f;		// time on lunging

    public override void Entry()
    {
		Debug.Log (this.stateMachine.NPC.name + ": Lunge entry");

		lungeTimer = GameManager.LUNGE_TIME;
    }

    public override NPCState Update()
    {
        lungeTimer -= Time.deltaTime;

        if (lungeTimer > 0)
        {
            NPCActions.Lunge(this.stateMachine.NPC.Instance);
        }
        else
        {
            this.stateMachine.NPC.GetComponent<Rigidbody>().velocity = Vector3.zero;
            return new GuardSearchState(this.stateMachine);
        }

        return this;
    }
}
