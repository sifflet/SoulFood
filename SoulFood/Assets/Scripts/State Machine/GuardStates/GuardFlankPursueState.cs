using UnityEngine;
using System.Collections;

public class GuardFlankPursueState : GuardPursueState
{
    public GuardFlankPursueState(NPCStateMachine stateMachine)
        : base(stateMachine)
    {
    }

    public override void Entry()
    {
		Debug.Log (this.stateMachine.NPC.name + ": Flank Pursue entry");
        base.Entry();
    }

    public override NPCState Update()
    {
        NPCState stateFromBase = base.Update();
        if (stateFromBase != this) return stateFromBase;

        if (NPCStateHelper.GetShortestPathDistance(stateMachine.NPC.Instance, (stateMachine as GuardStateMachine).TargetNPC.Instance) <= GameManager.DIRECT_PURSUE_RANGE) return new GuardDirectPursueState(stateMachine);

        if ((stateMachine.NPC as GuardDriver).IsLeader)
        {
            NPCStateHelper.MoveTo(stateMachine.NPC, (stateMachine as GuardStateMachine).TargetNPC.Instance, 1f);
        }
        else
        {
            stateMachine.NPC.MovementDriver.ChangePathToFlank(NPCStateHelper.FindClosestNode((stateMachine as GuardStateMachine).TargetNPC.Instance), otherGuard);
        }
        
        return this;
    }
}
