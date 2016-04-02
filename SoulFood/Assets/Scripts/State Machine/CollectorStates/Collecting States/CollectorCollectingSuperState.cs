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

		if (CollectorStateHelper.GuardsInFleeRange(this.stateMachine, GameManager.FleeRangeType.Emergency)) ; // return emergencyFlee state
		if (CollectorStateHelper.GuardsInFleeRange(this.stateMachine, GameManager.FleeRangeType.Default)) return new CollectorFleeState(this.stateMachine); // return flee state

		// If you're close to a soul, you need to move to CollectSoul state (quick change)

        return this.stateMachine.CurrentState;
    }

}
