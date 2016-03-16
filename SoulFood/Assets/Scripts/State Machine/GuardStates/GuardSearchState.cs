﻿using UnityEngine;
using System.Collections;

public class GuardSearchState : NPCState
{
    public GuardSearchState(NPCStateMachine stateMachine)
        : base(stateMachine)
    {
    }

    public override void Entry()
    {
        NPCMovementDriver movementDriver = this.stateMachine.NPC.MovementDriver;
        Node newEndNode = movementDriver.AllNodes[UnityEngine.Random.Range(0, movementDriver.AllNodes.Count - 1)];
        movementDriver.ChangePath(newEndNode);
    }

    public override NPCState Update()
    {
        if (this.stateMachine.NPC.VisibleNPCs.Count > 0)
        {
            return new GuardPursueState(this.stateMachine);
        }

        NPCMovementDriver movementDriver = this.stateMachine.NPC.MovementDriver;

        if (movementDriver.AttainedFinalNode)
        {
            Node newEndNode = movementDriver.AllNodes[UnityEngine.Random.Range(0, movementDriver.AllNodes.Count - 1)];
            movementDriver.ChangePath(newEndNode);
        }

        return this;
    }
}