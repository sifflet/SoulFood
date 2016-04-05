using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectorFindSingleTreeState : CollectorCollectingSuperState {

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

		movementDriver = this.stateMachine.NPC.MovementDriver;
		
		GameObject buttonTargetForClosestSingleTree = CollectorStateHelper.FindClosestFullTreeButton(this.stateMachine.NPC, 1); 

		if (buttonTargetForClosestSingleTree) 
		{
			
			if (NPCStateHelper.IsWithinCollisionRangeAtGroundLevel(stateMachine.NPC.Instance, buttonTargetForClosestSingleTree)) {

				return this;
			}

            NPCStateHelper.MoveTo(this.stateMachine.NPC, buttonTargetForClosestSingleTree, 5f);
           
		}
		else 
		{
			// If we're at the end of our path having found no souls, find a new random one
			if (movementDriver.AttainedFinalNode)
			{
				Node newEndNode = GameManager.AllNodes[UnityEngine.Random.Range(0, GameManager.AllNodes.Count - 1)];
				movementDriver.ChangePath(newEndNode);
			}
			// TODO: Add timer to stop wandering and
			// return FindMultiplayerTreeState
		}

		return this;
	}
}
