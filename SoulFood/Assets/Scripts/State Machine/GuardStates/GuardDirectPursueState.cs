using UnityEngine;
using System.Collections;

public class GuardDirectPursueState : GuardPursueState
{
    public GuardDirectPursueState(NPCStateMachine stateMachine)
        : base(stateMachine)
    {
    }

    public override void Entry()
    {
		Debug.Log (this.stateMachine.NPC.name + ": Direct Pursue entry");
        base.Entry();
    }

    public override NPCState Update()
    {
        NPCState stateFromBase = base.Update();
        if(stateFromBase != this) return stateFromBase;

        if (NPCStateHelper.GetShortestPathDistance(stateMachine.NPC.Instance, (stateMachine as GuardStateMachine).TargetNPC.Instance) > GameManager.DIRECT_PURSUE_RANGE) return new GuardFlankPursueState(stateMachine);

        NPCStateHelper.MoveTo(stateMachine.NPC, (stateMachine as GuardStateMachine).TargetNPC.Instance, 1f);

        return this;
    }
}
