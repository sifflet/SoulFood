using UnityEngine;
using System.Collections;

public class GuardLungeState : NPCState
{
    public GuardLungeState(NPCStateMachine stateMachine)
        : base(stateMachine)
    {
    }

    public override void Entry() { }
    public override NPCState Update() { return this; }
}
