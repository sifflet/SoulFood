using UnityEngine;
using System.Collections;

public class GuardPursueState : NPCState
{
    public GuardPursueState(NPCStateMachine stateMachine)
        : base(stateMachine)
    {
    }

    public override void Entry()
    {
        Debug.Log("Pursue State Entry");
        NPCMovementDriver movementDriver = this.stateMachine.NPC.MovementDriver;
        Node newEndNode = null;

        if (this.stateMachine.NPC.VisibleNPCs[0].MovementDriver.CurrentTargetNode == null)
        {
            movementDriver.ChangePath(this.stateMachine.NPC.VisibleNPCs[0].MovementDriver.FindClosestNode());
        }
        else
        {
            movementDriver.ChangePath(newEndNode);
        }
    }

    public override NPCState Update() { return this; }
}
