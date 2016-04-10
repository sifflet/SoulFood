using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectorImmortalState : NPCState
{
    private List<NPCDriver> guardsInSight;
    private float immortalTimer = 0f;

    public CollectorImmortalState(NPCStateMachine stateMachine)
        : base(stateMachine)
    {
    }

    public override void Entry()
    {
		// Add state on to stack
		this.stateMachine.PushStateOnStack(this);

        Debug.Log(this.stateMachine.NPC.name + ": Immortal state entry");
        this.immortalTimer = CollectorStateMachine.IMMORTALITY_TIME;
        this.guardsInSight = CollectorStateHelper.FindGuardsInSight(this.stateMachine);
        this.stateMachine.NPC.MovementDriver.ChangePathToFlee(CollectorStateMachine.FLEE_RANGE, guardsInSight);
    }

    public override NPCState Update()
    {
        immortalTimer -= Time.deltaTime;

        if (immortalTimer <= 0) return new CollectorSearchSoulsState(this.stateMachine);

        if (this.stateMachine.NPC.MovementDriver.AttainedFinalNode)
        {
            this.stateMachine.NPC.MovementDriver.ChangePathToFlee(CollectorStateMachine.FLEE_RANGE, guardsInSight);
        }

        return this;
    }
}
