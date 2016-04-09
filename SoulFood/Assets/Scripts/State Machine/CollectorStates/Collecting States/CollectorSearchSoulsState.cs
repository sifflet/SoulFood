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
		Debug.Log (this.stateMachine.NPC.name + ": Search Soul State Entry");
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

		visibleSouls = CollectorStateHelper.FindVisibleSouls(this.stateMachine.NPC);
		if (visibleSouls.Count > 0) { 		// If there are visible souls
			GameObject closestSoul = NPCStateHelper.FindClosestGameObject(this.stateMachine.NPC.gameObject, visibleSouls);

			if (!NPCStateHelper.IsWithinCollisionRangeAtGroundLevel(this.stateMachine.NPC.Instance, closestSoul, GameManager.COLLISION_RANGE))
			{
				NPCStateHelper.MoveTo(this.stateMachine.NPC, closestSoul, 1f);
			}			
		}
		else {	// There are no visible souls
			if (soulSearchingTimer > 0) // If the time period for soul searching is not over
			{
				// If we're at the end of our path having found no souls, find a new random one
				CollectorStateHelper.GetNewRandomPath(movementDriver);
			}
			else {	// If the time period for soul searching is over
				return new CollectorFindSingleTreeState(this.stateMachine);
			}
		}

        return this;
    }
}
