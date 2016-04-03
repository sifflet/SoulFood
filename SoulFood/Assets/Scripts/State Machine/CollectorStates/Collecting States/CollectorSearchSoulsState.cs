using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectorSearchSoulsState : CollectorCollectingSuperState
{
	private float soulSearchingTimer = GameManager.TIME_SPENT_SOUL_SEARCHING;
	private List<GameObject> visibleSouls = new List<GameObject>();

	private NPCMovementDriver movementDriver;

    public CollectorSearchSoulsState(NPCStateMachine stateMachine)
        : base(stateMachine)
    {
    }

    public override void Entry()
    {
        movementDriver = this.stateMachine.NPC.MovementDriver;
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

		soulSearchingTimer -= Time.deltaTime;
        movementDriver = this.stateMachine.NPC.MovementDriver;

		if (soulSearchingTimer > 0) {	// If the time period for soul searching is not over

			// Move to closest soul
			visibleSouls = CollectorStateHelper.FindVisibleSouls(this.stateMachine.NPC);
			if (visibleSouls.Count > 0) {
				GameObject closestSoul = NPCStateHelper.FindClosestGameObject(this.stateMachine.NPC.gameObject, visibleSouls);
							
				// Then, once the NPC is at the soul, stop movement and transition to CollectSouls state
				if (NPCStateHelper.IsColliding(this.stateMachine.NPC, closestSoul))
				{
					return new CollectorCollectSoulsState(this.stateMachine);
				}
				else { // If not yet at target, keep moving
					NPCStateHelper.MoveTo(this.stateMachine.NPC, closestSoul, 1f);
				}
			
			}

			// If we're at the end of our path having found no souls, find a new random one
	        if (movementDriver.AttainedFinalNode)
	        {
	            Node newEndNode = GameManager.AllNodes[UnityEngine.Random.Range(0, GameManager.AllNodes.Count - 1)];
	            movementDriver.ChangePath(newEndNode);
	        }
		}
		else {	// Time period for soul searching is over
			return new CollectorFindSingleTreeState(this.stateMachine);
		}

        return this;
    }
}
