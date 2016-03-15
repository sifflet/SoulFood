using UnityEngine;
using System.Collections;

public class CollectorStateMachine : NPCStateMachine
{
    public CollectorStateMachine(NPCDriver npc)
        : base(npc)
    {
        this.currentState = null;
    }

    public override void Reset()
    {
        this.currentState = null;
    }
}
