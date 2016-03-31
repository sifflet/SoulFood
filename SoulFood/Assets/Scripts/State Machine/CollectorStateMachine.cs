using UnityEngine;
using System.Collections;

public class CollectorStateMachine : NPCStateMachine
{
    private float fleeRange = 20.0f;
    private float emergencyFleeRange = 10.0f;

    public float FleeRange { get { return this.fleeRange; } }
    public float EmergencyFleeRange { get { return this.emergencyFleeRange; } }

    public override void Setup(NPCDriver npc)
    {
        base.Setup(npc);
        this.currentState = new CollectorSearchSoulsState(this);
    }

    public override void Reset()
    {
        this.currentState = new CollectorSearchSoulsState(this);
    }
}
