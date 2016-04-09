using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectorFindSingleTreeState : CollectorCollectingSuperState {

	private float singleTreeSearchingTimer = GameManager.TIME_SPENT_SINGLE_TREE_SEARCHING;
	private NPCMovementDriver movementDriver;
	
	public CollectorFindSingleTreeState(NPCStateMachine stateMachine)
		: base(stateMachine)
	{
	}
	
	public override void Entry()
	{
		Debug.Log (this.stateMachine.NPC.name + ": Find Single Tree State Entry");
		movementDriver = this.stateMachine.NPC.MovementDriver;
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

		singleTreeSearchingTimer -= Time.deltaTime;
		movementDriver = this.stateMachine.NPC.MovementDriver;
		
		GameObject buttonTargetForClosestSingleTree = CollectorStateHelper.FindClosestFullTreeButton(this.stateMachine.NPC, 1); 

		if (buttonTargetForClosestSingleTree) 
		{
			
			if (NPCStateHelper.IsWithinCollisionRangeAtGroundLevel(stateMachine.NPC.Instance, buttonTargetForClosestSingleTree, GameManager.COLLISION_RANGE)) {

				return this;
			}

            NPCStateHelper.MoveTo(this.stateMachine.NPC, buttonTargetForClosestSingleTree, 5f);
           
		}
		else 
		{
			if (singleTreeSearchingTimer > 0) {
				// If we're at the end of our path having found no souls, find a new random one
				CollectorStateHelper.GetNewRandomPath(movementDriver);
			}
			else {
				return new CollectorFindMultipleTreeState(this.stateMachine);
			}
		}

		return this;
	}
}
