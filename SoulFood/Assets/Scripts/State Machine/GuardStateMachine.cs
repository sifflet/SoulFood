using UnityEngine;
using System.Collections;

public class GuardStateMachine : NPCStateMachine
{
    public override void Setup(NPCDriver npc)
    {
        base.Setup(npc);
        this.currentState = new GuardSearchState(this);
    }

    public override void Reset()
    {
        this.currentState = new GuardSearchState(this);
    }
}
