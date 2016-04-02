using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectorSearchSoulsState : CollectorCollectingState
{
	private float soulSearchingTimer = GameManager.TIME_SPENT_SOUL_SEARCHING;

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
		// Check if guards are in sight and if a transition to flee states is necessary
		// These checks are in the base state
		NPCState stateFromBase = base.Update();
		if (stateFromBase != this)
		{
			return stateFromBase;
		}

        NPCMovementDriver movementDriver = this.stateMachine.NPC.MovementDriver;

        if (movementDriver.AttainedFinalNode)
        {
            Node newEndNode = GameManager.AllNodes[UnityEngine.Random.Range(0, GameManager.AllNodes.Count - 1)];
            movementDriver.ChangePath(newEndNode);
        }

        return this;
    }
}
