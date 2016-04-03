using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectorFindSingleTreeState : CollectorCollectingSuperState {

	private NPCMovementDriver movementDriver;
	private List<GameObject> visibleSouls = new List<GameObject>();
	private float waitTimeForSoulReleaseFromTree = 2f;
	
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

		if (CollectorStateHelper.HasVisibleSouls(stateMachine.NPC)) return new CollectorCollectSoulsState(this.stateMachine); // return soul collecting state

		movementDriver = this.stateMachine.NPC.MovementDriver;
		
		GameObject buttonTargetForClosestSingleTree = CollectorStateHelper.FindClosestFullTreeButton(this.stateMachine.NPC, 1); 

		if (buttonTargetForClosestSingleTree) 
		{
			
			if (NPCStateHelper.IsWithinCollisionRangeAtGroundLevel(stateMachine.NPC.Instance, buttonTargetForClosestSingleTree)) {

				//if (CollectorStateHelper.HasVisibleSouls(stateMachine.NPC)) return new CollectorCollectSoulsState(this.stateMachine); // return soul collecting state

				return this;
			}

            NPCStateHelper.MoveTo(this.stateMachine.NPC, buttonTargetForClosestSingleTree, 5f);
           
		}
		else 
		{
			// return FindMultiplayerTreeState
		}

		return this;
	}
}
