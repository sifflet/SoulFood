﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectorSearchSoulsState : CollectorCollectingState
{
    public CollectorSearchSoulsState(NPCStateMachine stateMachine)
        : base(stateMachine)
    {
    }

    public override void Entry()
    {
        NPCMovementDriver movementDriver = this.stateMachine.NPC.MovementDriver;
        Node newEndNode = GameManager.AllNodes[UnityEngine.Random.Range(0, GameManager.AllNodes.Count - 1)];
        movementDriver.ChangePath(newEndNode);
    }

    public override NPCState Update()
    {
		this.guardsInSight = CollectorStateHelper.FindGuardsInSight(this.stateMachine);

		if (CollectorStateHelper.GuardsInFleeRange(this.stateMachine, "emergency")) ; // return emergencyFlee state
		if (CollectorStateHelper.GuardsInFleeRange(this.stateMachine, "default")) return new CollectorFleeState(this.stateMachine); // return flee state
		
        NPCMovementDriver movementDriver = this.stateMachine.NPC.MovementDriver;

        if (movementDriver.AttainedFinalNode)
        {
            Node newEndNode = GameManager.AllNodes[UnityEngine.Random.Range(0, GameManager.AllNodes.Count - 1)];
            movementDriver.ChangePath(newEndNode);
        }

        return base.Update();
    }
}
