﻿using UnityEngine;
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
		//buttonTargetForClosestDoubleTree = CollectorStateHelper.FindClosestFullTreeButton(this.stateMachine.NPC, 2);
		//buttonTargetForClosestTripleTree = CollectorStateHelper.FindClosestFullTreeButton(this.stateMachine.NPC, 3); 

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

        buttonTargetForClosestDoubleTree = CollectorStateHelper.FindClosestFullTreeButton(this.stateMachine.NPC, 2);
        buttonTargetForClosestTripleTree = CollectorStateHelper.FindClosestFullTreeButton(this.stateMachine.NPC, 3); 
		
		multipleTreeSearchingTimer -= Time.deltaTime;
		movementDriver = this.stateMachine.NPC.MovementDriver;
		
		if (buttonTargetForClosestDoubleTree) 	// See if a double tree is found first
		{
			
			if (NPCStateHelper.IsWithinCollisionRangeAtGroundLevel(this.stateMachine.NPC.Instance, buttonTargetForClosestDoubleTree, CollectorStateMachine.TREE_MOVEMENT_COLLISION_RANGE)) {
				SoulTree targetTree = buttonTargetForClosestDoubleTree.GetComponent<Button>().GetSoulTreeForCurrentButton(); 
				return new CollectorCallForHelpState(this.stateMachine, targetTree);
			}
			else {			
				NPCStateHelper.MoveTo(this.stateMachine.NPC, buttonTargetForClosestDoubleTree, 5f);
			}
			
		}
		else if (buttonTargetForClosestTripleTree) 	// If no double tree, see if a triple tree is found
		{
			if (NPCStateHelper.IsWithinCollisionRangeAtGroundLevel(this.stateMachine.NPC.Instance, buttonTargetForClosestTripleTree, CollectorStateMachine.TREE_MOVEMENT_COLLISION_RANGE)) {
				SoulTree targetTree = buttonTargetForClosestTripleTree.GetComponent<Button>().GetSoulTreeForCurrentButton(); 
				return new CollectorCallForHelpState(this.stateMachine, targetTree);
			}
			else {
				NPCStateHelper.MoveTo(this.stateMachine.NPC, buttonTargetForClosestTripleTree, 5f);
			}
		}
		else 	// No double or triple trees were found
		{
			if (multipleTreeSearchingTimer > 0) {
				// If we're at the end of our path having found nothing, find a new random one
				CollectorStateHelper.GetNewRandomPath(movementDriver); 
			}
			else {
				return new CollectorSearchSoulsState(this.stateMachine);
			}
		}
		
		return this;
	}
}
