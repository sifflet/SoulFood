using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class CollectorCollectingState : NPCState
{
    protected List<NPCDriver> guardsInSight;

    public CollectorCollectingState(NPCStateMachine stateMachine)
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

		if (CollectorStateHelper.GuardsInFleeRange(this.stateMachine, "emergency")) ; // return emergencyFlee state
		if (CollectorStateHelper.GuardsInFleeRange(this.stateMachine, "default")) return new CollectorFleeState(this.stateMachine); // return flee state

        return this.stateMachine.CurrentState;
    }

}
