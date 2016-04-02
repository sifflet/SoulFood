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
        Debug.Log("Flank Pursue entry");
        base.Entry();
    }

    public override NPCState Update()
    {
        NPCState stateFromBase = base.Update();
        if (stateFromBase != this) return stateFromBase;

        if (NPCStateHelper.GetShortestPathDistance(stateMachine.NPC.Instance, targetNPC.Instance) <= GameManager.DIRECT_PURSUE_RANGE) return new GuardDirectPursueState(stateMachine);

        // implement flanking
        NPCStateHelper.MoveTo(stateMachine.NPC, targetNPC.Instance, 5f);
        
        return this;
    }
}
