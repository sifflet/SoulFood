using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectorCollectSoulsState : CollectorCollectingSuperState {

	private NPCMovementDriver movementDriver;
	private List<GameObject> visibleSouls = new List<GameObject>();
	
	public CollectorCollectSoulsState(NPCStateMachine stateMachine)
		: base(stateMachine)
	{
	}
	
	public override void Entry()
	{
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

		visibleSouls = CollectorStateHelper.FindVisibleSouls(this.stateMachine.NPC);

		if (visibleSouls.Count > 0) {
			GameObject closestSoul = NPCStateHelper.FindClosestGameObject(this.stateMachine.NPC.gameObject, visibleSouls);

			if (NPCStateHelper.IsColliding(this.stateMachine.NPC, closestSoul))
			{
				NPCActions.ConsumeSoul((CollectorDriver)this.stateMachine.NPC, closestSoul);
			}
			else 
			{
				NPCStateHelper.MoveTo(this.stateMachine.NPC, closestSoul, 1f);			
			}
		}
		else {	// All visible souls have been collected, return to soul search state
			return new CollectorSearchSoulsState(this.stateMachine);
		}
		
		return this;
	}
}
