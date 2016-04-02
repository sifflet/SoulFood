using UnityEngine;
using System.Collections;

public class GuardFlankPursueState : GuardPursueState
{
    private NPCDriver otherGuard;

    public GuardFlankPursueState(NPCStateMachine stateMachine)
        : base(stateMachine)
    {
    }

    public override void Entry()
    {
        Debug.Log("Flank Pursue entry");
        otherGuard = FindOtherGuard();
        base.Entry();
    }

    public override NPCState Update()
    {
        NPCState stateFromBase = base.Update();
        if (stateFromBase != this) return stateFromBase;

        if (NPCStateHelper.GetShortestPathDistance(stateMachine.NPC.Instance, targetNPC.Instance) <= GameManager.DIRECT_PURSUE_RANGE) return new GuardDirectPursueState(stateMachine);

        stateMachine.NPC.MovementDriver.ChangePathToFlank(NPCStateHelper.FindClosestNode(targetNPC.Instance), otherGuard);
        
        return this;
    }

    private NPCDriver FindOtherGuard()
    {
        foreach (NPCDriver npc in GameManager.Guards)
        {
            if (npc == this.stateMachine.NPC) continue;

            return npc;
        }

        return null;
    }
}
