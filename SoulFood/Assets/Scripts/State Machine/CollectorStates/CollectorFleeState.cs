using UnityEngine;
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
		this.guardsInSight = CollectorStateHelper.FindGuardsInSight(this.stateMachine);
		List<NPCDriver> guardsInFleeRange = CollectorStateHelper.FindGuardsInFleeRange(this.stateMachine);
        this.stateMachine.NPC.MovementDriver.ChangePathToFlee((this.stateMachine as CollectorStateMachine).FleeRange, guardsInFleeRange);
    }

    public override NPCState Update()
    {
		this.guardsInSight = CollectorStateHelper.FindGuardsInSight(this.stateMachine);
		List<NPCDriver> guardsInFleeRange = CollectorStateHelper.FindGuardsInFleeRange(this.stateMachine);

        if (guardsInSight.Count == 0) return new CollectorSearchSoulsState(this.stateMachine);
        if (guardsInFleeRange.Count == 0) return new CollectorSearchSoulsState(this.stateMachine);
		if (CollectorStateHelper.GuardsInFleeRange(this.stateMachine, GameManager.FleeRangeType.Emergency)) ; // return emergency flee state

        if (this.stateMachine.NPC.MovementDriver.AttainedFinalNode)
        {
            this.stateMachine.NPC.MovementDriver.ChangePathToFlee((this.stateMachine as CollectorStateMachine).FleeRange, guardsInFleeRange);
        }

        return this;
    }
}
