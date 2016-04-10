using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectorAnswerHelpCallState : CollectorCollectingSuperState {

	private float answerHelpCallTimer = CollectorStateMachine.TIME_SPENT_WAITING_FOR_TREE_HELP;
	private NPCMovementDriver movementDriver;
	private SoulTree targetTree;
	private CollectorStateMachine callerStateMachine;	// State machine of the caller
	private bool hasNotifiedCallerOfArrival = false;
	
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
		
		// Check if help call has been cancelled
		// If so, returned to soul search state
		if (!(this.stateMachine as CollectorStateMachine).HasReceivedHelpCall) {
			return new CollectorSearchSoulsState(this.stateMachine);
		}

        if (hasNotifiedCallerOfArrival)
        {
            answerHelpCallTimer -= Time.deltaTime;
            if (answerHelpCallTimer < 0) return new CollectorSearchSoulsState(this.stateMachine); // TODO: Replace this with stack call to previous state
        }

		movementDriver = this.stateMachine.NPC.MovementDriver;

		// Get a button target for the targetTree
		GameObject buttonTarget = GetUntriggeredButtonFromTree(targetTree);

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
			if (!buttonObj.GetComponent<Button>().IsTriggered)
				targetButton = buttonObj;
		}

		return targetButton;
	}
}
