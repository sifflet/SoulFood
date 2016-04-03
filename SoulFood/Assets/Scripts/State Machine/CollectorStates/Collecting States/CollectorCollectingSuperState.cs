using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class CollectorCollectingSuperState : NPCState
{
    protected List<NPCDriver> guardsInSight;

    public CollectorCollectingSuperState(NPCStateMachine stateMachine)
        : base(stateMachine)
    {
        this.guardsInSight = new List<NPCDriver>();
    }

    public override void Entry()
    {
    }

    public override NPCState Update()
    {
		this.guardsInSight = CollectorStateHelper.FindGuardsInSight(this.stateMachine);

		// If you're close to a guard, you need to move to flee state (quick change)
		if (CollectorStateHelper.GuardsInFleeRange(this.stateMachine, GameManager.FleeRangeType.Emergency)) ; // return emergencyFlee state
		if (CollectorStateHelper.GuardsInFleeRange(this.stateMachine, GameManager.FleeRangeType.Default)) return new CollectorFleeState(this.stateMachine); // return flee state

		if (CollectorStateHelper.SoulsInCollectibleRange(this.stateMachine)) return new CollectorCollectSoulsState(this.stateMachine); // return soul collecting state


		return this.stateMachine.CurrentState;
    }

}
