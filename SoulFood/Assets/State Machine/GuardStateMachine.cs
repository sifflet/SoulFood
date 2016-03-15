using UnityEngine;
using System.Collections;

public class GuardStateMachine : NPCStateMachine
{
    public GuardStateMachine(NPCDriver npc)
        : base(npc)
    {
        this.currentState = new GuardSearchState(this);
    }

    public override void Reset()
    {
        this.currentState = new GuardSearchState(this);
    }
}
