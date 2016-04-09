using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectorFindMultipleTreeState : CollectorCollectingSuperState {

	private float multipleTreeSearchingTimer = GameManager.TIME_SPENT_MULTIPLE_TREE_SEARCHING;
	private NPCMovementDriver movementDriver;

	public CollectorFindMultipleTreeState(NPCStateMachine stateMachine)
		: base(stateMachine)
	{
	}
	
	public override void Entry()
	{
		Debug.Log (this.stateMachine.NPC.name + ": Find Multiple Tree State Entry");
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
		
		multipleTreeSearchingTimer -= Time.deltaTime;
		movementDriver = this.stateMachine.NPC.MovementDriver;
		
		GameObject buttonTargetForClosestDoubleTree = CollectorStateHelper.FindClosestFullTreeButton(this.stateMachine.NPC, 2);
		GameObject buttonTargetForClosestTripleTree = CollectorStateHelper.FindClosestFullTreeButton(this.stateMachine.NPC, 3); 
		
		if (buttonTargetForClosestDoubleTree) 
		{
			
			if (NPCStateHelper.IsWithinCollisionRangeAtGroundLevel(stateMachine.NPC.Instance, buttonTargetForClosestDoubleTree, GameManager.COLLISION_RANGE)) {
				
				return new CollectorCallForHelpState(this.stateMachine);
			}
			
			NPCStateHelper.MoveTo(this.stateMachine.NPC, buttonTargetForClosestDoubleTree, 5f);
			
		}
		else if (buttonTargetForClosestTripleTree) 
		{

		}
		else 
		{
			if (multipleTreeSearchingTimer > 0) {
				// If we're at the end of our path having found no souls, find a new random one
				CollectorStateHelper.GetNewRandomPath(movementDriver); 
			}
			else {
				//return new CollectorFindMultipleTreeState(this.stateMachine);
			}
		}
		
		return this;
	}
}
