using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectorAnswerHelpCallState : CollectorCollectingSuperState {

	private float answerHelpCallTimer = CollectorStateMachine.TIME_SPENT_WAITING_FOR_TREE_HELP;
	private NPCMovementDriver movementDriver;
	private SoulTree targetTree;
	
	public CollectorAnswerHelpCallState(NPCStateMachine stateMachine, SoulTree targetTree)
		: base(stateMachine)
	{
		this.targetTree = targetTree;
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

		answerHelpCallTimer -= Time.deltaTime;
		movementDriver = this.stateMachine.NPC.MovementDriver;

		// Get a button target for the targetTree
		GameObject buttonTarget = GetUntriggeredButtonFromTree(targetTree);

		if (buttonTarget) {
			if (NPCStateHelper.IsWithinCollisionRangeAtGroundLevel(stateMachine.NPC.Instance, buttonTarget, CollectorStateMachine.TREE_COLLISION_RANGE)) {
				(this.stateMachine as CollectorStateMachine).CancelTreeHelpCall();
				return this;
			}
			
			NPCStateHelper.MoveTo(this.stateMachine.NPC, buttonTarget, 5f);
		}
		else 
		{
			if (answerHelpCallTimer < 0) {
				return new CollectorSearchSoulsState(this.stateMachine); // TODO: Replace this with stack call to previous state
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
