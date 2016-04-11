﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectorFindSingleTreeState : CollectorCollectingSuperState {

	private float singleTreeSearchingTimer = CollectorStateMachine.TIME_SPENT_SINGLE_TREE_SEARCHING;
	private NPCMovementDriver movementDriver;
	private GameObject buttonTargetForClosestSingleTree;
	
	public CollectorFindSingleTreeState(NPCStateMachine stateMachine)
		: base(stateMachine)
	{
	}
	
	public override void Entry()
	{
		Debug.Log (this.stateMachine.NPC.name + ": Find Single Tree State Entry");
		movementDriver = this.stateMachine.NPC.MovementDriver;
		buttonTargetForClosestSingleTree = CollectorStateHelper.FindClosestFullTreeButton(this.stateMachine.NPC, 1); 
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

		// Add trees seen to strategic tree memory system
		this.stateMachine.AddVisibleTrees(NPCStateHelper.FindVisibleTrees(stateMachine.NPC));

		if (buttonTargetForClosestSingleTree) 
		{
			
			if (NPCStateHelper.IsWithinCollisionRangeAtGroundLevel(stateMachine.NPC.Instance, buttonTargetForClosestSingleTree, CollectorStateMachine.TREE_MOVEMENT_COLLISION_RANGE)) {
				// Indicate that the button has been targetted
				Button buttonScript = buttonTargetForClosestSingleTree.GetComponent<Button>();
				buttonScript.IsTargettedForTriggering = true;

				// Since it is likely souls will now be released from the tree and caller is transitioned into collect soul state
				// We reset the stack history so that after the collect soul state, the caller returns to search for souls and not search for single tree again
				this.ResetStackToDefaultState(new CollectorSearchSoulsState(this.stateMachine));

				return this;
			}
			else {
            	NPCStateHelper.MoveTo(this.stateMachine.NPC, buttonTargetForClosestSingleTree, 5f);
			}
           
		}
		else 
		{
			if (singleTreeSearchingTimer > 0) {
				// If we're at the end of our path having found no trees, find a new path based on the trees we've remembered
				CollectorStateHelper.GetNewPathToGivenNode(movementDriver, CollectorStateHelper.FindNodeForRememberedTreePosition(this.stateMachine)); 
			}
			else {
				return new CollectorFindMultipleTreeState(this.stateMachine);
			}
		}

		return this;
	}
}
