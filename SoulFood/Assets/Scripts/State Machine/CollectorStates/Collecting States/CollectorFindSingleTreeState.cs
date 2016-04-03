using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectorFindSingleTreeState : CollectorCollectingSuperState {

	private NPCMovementDriver movementDriver;
	private List<GameObject> visibleSouls = new List<GameObject>();
	
	public CollectorFindSingleTreeState(NPCStateMachine stateMachine)
		: base(stateMachine)
	{
	}
	
	public override void Entry()
	{
		Debug.Log ("Find Single Tree State Entry");
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
		
		movementDriver = this.stateMachine.NPC.MovementDriver;
		
		GameObject buttonTargetForClosestSingleTree = CollectorStateHelper.FindClosestFullTreeButton(this.stateMachine.NPC, 1); 

		if (buttonTargetForClosestSingleTree) 
		{
			// If at target, release souls from single tree
			if (NPCStateHelper.IsColliding(this.stateMachine.NPC, buttonTargetForClosestSingleTree))
			{
				Debug.Log("I'm at the button");
			}
			else { // If not yet at target, keep moving
				NPCStateHelper.MoveTo(this.stateMachine.NPC, buttonTargetForClosestSingleTree, 1f);
			}
		}
		else 
		{
			// return FindMultiplayerTreeState
		}

		return this;
	}
}
