using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectorEmergencyFleeState : NPCState
{
	protected List<NPCDriver> guardsInSight;
	private CollectorDriver collectorDriver;
	private float dropAllSoulsTimer = 3f;		// After this amount of time in the state, all souls should be dropped for max speed
	
	public CollectorEmergencyFleeState(NPCStateMachine stateMachine)
		: base(stateMachine)
	{
	}
	
	public override void Entry()
	{
		// Add state on to stack
		this.stateMachine.PushStateOnStack(this);
		
		Debug.Log (this.stateMachine.NPC.name + ": Emergency Flee State Entry");
		this.guardsInSight = CollectorStateHelper.FindGuardsInSight(this.stateMachine);
		List<NPCDriver> guardsInFleeRange = CollectorStateHelper.FindGuardsInFleeRange(this.stateMachine);
		this.stateMachine.NPC.MovementDriver.ChangePathToFlee(CollectorStateMachine.FLEE_RANGE, guardsInFleeRange);
		this.collectorDriver = this.stateMachine.NPC as CollectorDriver;
	}
	
	public override NPCState Update()
	{
		this.guardsInSight = CollectorStateHelper.FindGuardsInSight(this.stateMachine);
		List<NPCDriver> guardsInFleeRange = CollectorStateHelper.FindGuardsInFleeRange(this.stateMachine);

		dropAllSoulsTimer -= Time.deltaTime;

		// If guards are no longer in sight
		if (guardsInSight.Count == 0) return this.GetPreviousStateInStack();
		if (guardsInFleeRange.Count == 0) return this.GetPreviousStateInStack();

		if ( dropAllSoulsTimer > 0 )	// If it is not time to drop all the souls, drop around half
		{
			int numSoulsToDrop = (int) Mathf.Floor(this.collectorDriver.SoulsStored / 2);
			CollectorStateHelper.DropSouls(this.collectorDriver, numSoulsToDrop);
		}
		else 	// It's time to drop all your souls!!
		{		
			CollectorStateHelper.DropSouls(this.collectorDriver, collectorDriver.SoulsStored);
		}
		
		if (this.stateMachine.NPC.MovementDriver.AttainedFinalNode)
		{
			this.stateMachine.NPC.MovementDriver.ChangePathToFlee(CollectorStateMachine.FLEE_RANGE, guardsInFleeRange);
		}
		
		return this;
	}
}
