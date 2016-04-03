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
		Debug.Log (this.stateMachine.NPC.name + ": Collect Soul State Entry");
		movementDriver = this.stateMachine.NPC.MovementDriver;
	}
	
	public override NPCState Update()
	{
		// We can't call the super state's update here since the super state will call this child state and we will be stuck calling each other
		// Thus. the guard check and flee transitions are repeated here instead of using the base.update() call
		this.guardsInSight = CollectorStateHelper.FindGuardsInSight(this.stateMachine);
		
		// If you're close to a guard, you need to move to flee state (quick change)
		if (CollectorStateHelper.GuardsInFleeRange(this.stateMachine, GameManager.FleeRangeType.Emergency)) ; // return emergencyFlee state
		if (CollectorStateHelper.GuardsInFleeRange(this.stateMachine, GameManager.FleeRangeType.Default)) return new CollectorFleeState(this.stateMachine); // return flee state


		movementDriver = this.stateMachine.NPC.MovementDriver;

		visibleSouls = CollectorStateHelper.FindVisibleSouls(this.stateMachine.NPC);
	
		if (visibleSouls.Count > 0) {
			GameObject closestSoul = NPCStateHelper.FindClosestGameObject(this.stateMachine.NPC.gameObject, visibleSouls);

			if (NPCStateHelper.IsWithinCollisionRangeAtGroundLevel(this.stateMachine.NPC.Instance, closestSoul))
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
