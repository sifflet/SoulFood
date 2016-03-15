using UnityEngine;
using System.Collections;

public class GuardStrategicPositionState : NPCState
{
    public GuardStrategicPositionState(NPCStateMachine stateMachine)
        : base(stateMachine)
    {
    }

    public override void Entry() { }
    public override NPCState Update() { return this; }
}
