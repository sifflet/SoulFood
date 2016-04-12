using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectorAnswerHelpCallState : CollectorCollectingSuperState {
	
	private NPCMovementDriver movementDriver;
	private SoulTree targetTree;
	private CollectorStateMachine callerStateMachine;	// State machine of the caller
	private bool hasNotifiedCallerOfArrival = false;
	private GameObject buttonTarget;	
	
	public CollectorAnswerHelpCallState(NPCStateMachine stateMachine, SoulTree targetTree, CollectorStateMachine callerStateMachine)
		: base(stateMachine)
	{
		this.targetTree = targetTree;
		this.callerStateMachine = callerStateMachine;
	}
	
	public override void Entry()
	{
		Debug.Log (this.stateMachine.NPC.name + ": Answer Help Call State Entry");
		movementDriver = this.stateMachine.NPC.MovementDriver;
		buttonTarget = GetUntriggeredButtonFromTree(targetTree);	// Get a button target for the targetTree
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

		// In the case that this help call was initiated by a player, check if the player is still triggering the tree
		// If not, cancel the help calls
		if (!(this.stateMachine as CollectorStateMachine).CheckIfPlayerIsTriggeringTheTree(this.targetTree)) 
		{
			(this.stateMachine as CollectorStateMachine).CancelHelpCallsAfterPlayerCall();
		}
		
		movementDriver = this.stateMachine.NPC.MovementDriver;
		
		// Check if help call has been cancelled
		// If so, returned to soul search state
		if (!(this.stateMachine as CollectorStateMachine).HasReceivedHelpCall && !(this.stateMachine as CollectorStateMachine).hasReceivedPlayerHelpCall) {
			return this.GetPreviousStateInStack(); 
		}

		if (buttonTarget) {	
			// This collision range is set to handle the collision of the NPC and the tree button for the purpsoe of notifiying help calls
			if (NPCStateHelper.IsWithinCollisionRangeAtGroundLevel(stateMachine.NPC.Instance, buttonTarget, CollectorStateMachine.TREE_COLLISION_RANGE)) {
				if (!hasNotifiedCallerOfArrival) {
					this.callerStateMachine.NotifyCallerOfHelpArrival(this.stateMachine.NPC as CollectorDriver); 	// Inform the caller of your arrival
					hasNotifiedCallerOfArrival = true;
				}
			}
			// This collision range is set to move the NPC directly onto the tree button
			if (NPCStateHelper.IsWithinCollisionRangeAtGroundLevel(stateMachine.NPC.Instance, buttonTarget, CollectorStateMachine.TREE_MOVEMENT_COLLISION_RANGE)) {
				return this;
			}
			else {
				NPCStateHelper.MoveTo(this.stateMachine.NPC, buttonTarget, 5f);
			}
		}
		
		return this;
	}

	private GameObject GetUntriggeredButtonFromTree(SoulTree targetTree) 
	{
		List<GameObject> treeButtons = targetTree.TreeButtons;
		GameObject targetButton = null;

		foreach (GameObject buttonObj in treeButtons) {
			Button buttonScript = buttonObj.GetComponent<Button>();
			if (!buttonScript.IsTargettedForTriggering && !buttonScript.IsTriggered)
			{
				buttonScript.IsTargettedForTriggering = true;
				targetButton = buttonObj;
				return targetButton;
			}
		}

		return targetButton;
	}
}
