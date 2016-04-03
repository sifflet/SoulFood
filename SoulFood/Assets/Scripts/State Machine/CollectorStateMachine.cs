using UnityEngine;
using System.Collections;

public class CollectorStateMachine : NPCStateMachine
{
	private float fleeRange = GameManager.FLEE_RANGE;
    private float emergencyFleeRange = GameManager.EMERGENCY_FLEE_RANGE;

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
