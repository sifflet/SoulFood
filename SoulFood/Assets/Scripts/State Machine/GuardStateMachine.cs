using UnityEngine;
using System.Collections;

public class GuardStateMachine : NPCStateMachine
{
    public NPCDriver TargetNPC { get; set; }

    public override void Setup(NPCDriver npc)
    {
        base.Setup(npc);
        this.TargetNPC = null;
        this.currentState = new GuardSearchState(this);
    }

    public override void Reset()
    {
        this.currentState = new GuardSearchState(this);
    }

    public void ChangeCurrentState(NPCState newState)
    {
        this.currentState = newState;
    }
}
