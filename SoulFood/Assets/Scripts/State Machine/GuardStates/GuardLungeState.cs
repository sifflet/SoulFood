using UnityEngine;
using System.Collections;

public class GuardLungeState : NPCState
{
    public GuardLungeState(NPCStateMachine stateMachine)
        : base(stateMachine)
    {
    }

    public override void Entry()
    {
		Debug.Log (this.stateMachine.NPC.name + ": Lunge entry");
    }

    public override NPCState Update()
    {
        return this;
    }
}
