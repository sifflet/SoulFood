using UnityEngine;
using System.Collections;

public class CollectorStateMachine : NPCStateMachine
{
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
