using UnityEngine;
using System.Collections;

public class GuardPursueState : NPCState
{
    protected NPCDriver targetNPC;
    protected Node targetNPCCurrentTargetNode;

    public GuardPursueState(NPCStateMachine stateMachine)
        : base(stateMachine)
    {
    }

    public override void Entry()
    {
        Debug.Log("Pursue State Entry");
        NPCMovementDriver thisNPCMovementDriver = this.stateMachine.NPC.MovementDriver;

        this.targetNPC = GetClosestVisibleCollector();
        this.targetNPCCurrentTargetNode = targetNPC.MovementDriver.CurrentTargetNode;

        if (targetNPC.MovementDriver.CurrentTargetNode == null)
        {
            thisNPCMovementDriver.ChangePath(targetNPC.MovementDriver.FindClosestNode());
        }
        else
        {
            thisNPCMovementDriver.ChangePath(this.targetNPCCurrentTargetNode);
        }
    }

    public override NPCState Update()
    {
        // flank
        // direct pursue
        // this class should be abstract

        NPCMovementDriver thisNPCMovementDriver = this.stateMachine.NPC.MovementDriver;

        if (targetNPC.MovementDriver.CurrentTargetNode == null)
        {
            thisNPCMovementDriver.ChangePath(targetNPC.MovementDriver.FindClosestNode());
        }
        else if (this.targetNPCCurrentTargetNode != targetNPC.MovementDriver.CurrentTargetNode)
        {
            this.targetNPCCurrentTargetNode = targetNPC.MovementDriver.CurrentTargetNode;
            this.stateMachine.NPC.MovementDriver.ChangePath(this.targetNPCCurrentTargetNode);
        }

        return this;
    }

    private NPCDriver GetClosestVisibleCollector()
    {
        NPCDriver result = this.stateMachine.NPC.VisibleNPCs[0];

        foreach (NPCDriver npc in this.stateMachine.NPC.VisibleNPCs)
        {
            Vector3 thisNPCPosition = this.stateMachine.NPC.Instance.transform.position;
            Vector3 otherNPCPosition = npc.Instance.transform.position;
            Vector3 currentClosestNPCPosition = result.Instance.transform.position;

            if (Vector3.Distance(thisNPCPosition, otherNPCPosition) < Vector3.Distance(thisNPCPosition, currentClosestNPCPosition))
            {
                result = npc;
            }
        }

        return result;
    }
}
