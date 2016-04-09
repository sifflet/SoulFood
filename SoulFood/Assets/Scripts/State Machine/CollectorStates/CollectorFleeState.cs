﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectorFleeState : NPCState
{
    protected List<NPCDriver> guardsInSight;

    public CollectorFleeState(NPCStateMachine stateMachine)
        : base(stateMachine)
    {
    }

    public override void Entry()
    {
		Debug.Log (this.stateMachine.NPC.name + ": Flee State Entry");
		this.guardsInSight = CollectorStateHelper.FindGuardsInSight(this.stateMachine);
		List<NPCDriver> guardsInFleeRange = CollectorStateHelper.FindGuardsInFleeRange(this.stateMachine);
		this.stateMachine.NPC.MovementDriver.ChangePathToFlee(CollectorStateMachine.FLEE_RANGE, guardsInFleeRange);
    }

    public override NPCState Update()
    {
		this.guardsInSight = CollectorStateHelper.FindGuardsInSight(this.stateMachine);
		List<NPCDriver> guardsInFleeRange = CollectorStateHelper.FindGuardsInFleeRange(this.stateMachine);

		// If guards are no longer in sight
		if (guardsInSight.Count == 0) return new CollectorSearchSoulsState(this.stateMachine); // TODO: Replace this with stack call to previous state
		if (guardsInFleeRange.Count == 0) return new CollectorSearchSoulsState(this.stateMachine); // TODO: Replace this with stack call to previous state

		// If guards are getting too close, emergency flee required
		if (CollectorStateHelper.GuardsInFleeRange(this.stateMachine, CollectorStateMachine.FleeRangeType.Emergency)) ; // TODO: return emergency flee state

        if (this.stateMachine.NPC.MovementDriver.AttainedFinalNode)
        {
            this.stateMachine.NPC.MovementDriver.ChangePathToFlee(CollectorStateMachine.FLEE_RANGE, guardsInFleeRange);
        }

        return this;
    }
}
