using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectorCollectingState : NPCState
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
        this.guardsInSight = FindGuardsInSight();

        if (GuardsInEmergencyFleeRange()) ; // return emergencyFlee state
        if (GuardsInFleeRange()) return new CollectorFleeState(this.stateMachine); // return flee state

        return this.stateMachine.CurrentState;
    }

}
