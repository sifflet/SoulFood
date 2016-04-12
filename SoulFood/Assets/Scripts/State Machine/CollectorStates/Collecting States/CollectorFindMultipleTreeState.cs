using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectorFindMultipleTreeState : CollectorCollectingSuperState {

	private float multipleTreeSearchingTimer = CollectorStateMachine.TIME_SPENT_MULTIPLE_TREE_SEARCHING;
	private NPCMovementDriver movementDriver;
	private GameObject buttonTargetForClosestDoubleTree;
	private GameObject buttonTargetForClosestTripleTree;

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

		// Add trees seen to strategic tree memory system
		this.stateMachine.AddVisibleTrees(NPCStateHelper.FindVisibleTrees(stateMachine.NPC));

		// Find buttons for multiple that are visible
		buttonTargetForClosestDoubleTree = CollectorStateHelper.FindClosestFullTreeButton(this.stateMachine.NPC, 2);
		buttonTargetForClosestTripleTree = CollectorStateHelper.FindClosestFullTreeButton(this.stateMachine.NPC, 3);

		if (multipleTreeSearchingTimer > 0) {

			if (buttonTargetForClosestDoubleTree) 	// See if a double tree is found first
			{
				if (NPCStateHelper.IsWithinCollisionRangeAtGroundLevel(this.stateMachine.NPC.Instance, buttonTargetForClosestDoubleTree, CollectorStateMachine.TREE_MOVEMENT_COLLISION_RANGE)) {
					// Indicate that the button has been targetted by the caller
					Button buttonScript = buttonTargetForClosestDoubleTree.GetComponent<Button>();
					buttonScript.IsTargettedForTriggering = true;

					SoulTree targetTree = buttonScript.GetSoulTreeForCurrentButton();
					return new CollectorCallForHelpState(this.stateMachine, targetTree);
				}
				else {			
					NPCStateHelper.MoveTo(this.stateMachine.NPC, buttonTargetForClosestDoubleTree, 5f);
				}
				
			}
			else if (buttonTargetForClosestTripleTree) 	// If no double tree, see if a triple tree is found
			{
				if (NPCStateHelper.IsWithinCollisionRangeAtGroundLevel(this.stateMachine.NPC.Instance, buttonTargetForClosestTripleTree, CollectorStateMachine.TREE_MOVEMENT_COLLISION_RANGE)) {
					// Indicate that the button has been targetted by the caller
					Button buttonScript = buttonTargetForClosestTripleTree.GetComponent<Button>();
					buttonScript.IsTargettedForTriggering = true;

					SoulTree targetTree = buttonTargetForClosestTripleTree.GetComponent<Button>().GetSoulTreeForCurrentButton(); 
					return new CollectorCallForHelpState(this.stateMachine, targetTree);
				}
				else {
					NPCStateHelper.MoveTo(this.stateMachine.NPC, buttonTargetForClosestTripleTree, 5f);
				}
			}
			else {
				// If we're at the end of our path having found no trees, find a new path based on the trees we've remembered
				//CollectorStateHelper.GetNewPath(movementDriver, GameManager.AllNodes[UnityEngine.Random.Range(0, GameManager.AllNodes.Count - 1)];
				CollectorStateHelper.GetNewPath(movementDriver, CollectorStateHelper.FindNodeForRememberedTreePosition(this.stateMachine)); 
			}
		}
		else {
			return new CollectorSearchSoulsState(this.stateMachine);
		}
		
		return this;
	}
}
