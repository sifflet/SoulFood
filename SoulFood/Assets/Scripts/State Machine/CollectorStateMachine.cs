using UnityEngine;
using System.Collections;

public class CollectorStateMachine : NPCStateMachine
{
    private float fleeRange = 20.0f;
    private float emergencyFleeRange = 10.0f;

    public float FleeRange { get { return this.fleeRange; } }
    public float EmergencyFleeRange { get { return this.emergencyFleeRange; } }

    public CollectorStateMachine(NPCDriver npc)
        : base(npc)
    {
        this.currentState = new CollectorSearchSoulsState(this);
    }

    public override void Reset()
    {
        this.currentState = new CollectorSearchSoulsState(this);
    }
}
