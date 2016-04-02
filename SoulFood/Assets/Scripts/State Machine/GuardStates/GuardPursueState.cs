using UnityEngine;
using System.Collections;

public abstract class GuardPursueState : NPCState
{
    protected NPCDriver targetNPC;
    protected Node targetNPCCurrentTargetNode;

    public GuardPursueState(NPCStateMachine stateMachine)
        : base(stateMachine)
    {
    }

    public override void Entry()
    {
        NPCMovementDriver thisNPCMovementDriver = this.stateMachine.NPC.MovementDriver;

        this.targetNPC = GetClosestVisibleCollector();
        this.targetNPCCurrentTargetNode = targetNPC.MovementDriver.CurrentTargetNode;

        NPCStateHelper.MoveTo(stateMachine.NPC, targetNPC.Instance, 5f);
    }

    public override NPCState Update()
    {
        if (NPCStateHelper.GetShortestPathDistance(stateMachine.NPC.Instance, targetNPC.Instance) <= GameManager.ACTIVATE_LUNGE_DISTANCE &&
            Vector3.Distance(stateMachine.NPC.Instance.transform.position, targetNPC.Instance.transform.position) <= GameManager.ACTIVATE_LUNGE_DISTANCE)
        {
            return new GuardLungeState(stateMachine);
        }

        if (stateMachine.NPC.VisibleNPCs.Count == 0) return new GuardSearchState(stateMachine);

        return this;
    }

    protected NPCDriver GetClosestVisibleCollector()
    {
        NPCDriver result = this.stateMachine.NPC.VisibleNPCs[0];
        float currentClosestDistance = NPCStateHelper.GetShortestPathDistance(stateMachine.NPC.Instance, stateMachine.NPC.VisibleNPCs[0].Instance);

        foreach (NPCDriver npc in this.stateMachine.NPC.VisibleNPCs)
        {
            GameObject thisNPC = this.stateMachine.NPC.Instance;
            GameObject otherNPC = npc.Instance;
            GameObject currentClosestNPC = result.Instance;

            float distanceToOtherNPC = NPCStateHelper.GetShortestPathDistance(thisNPC, otherNPC);
            if (distanceToOtherNPC < currentClosestDistance)
            {
                result = npc;
                currentClosestDistance = distanceToOtherNPC;
            }
        }

        return result;
    }
}
