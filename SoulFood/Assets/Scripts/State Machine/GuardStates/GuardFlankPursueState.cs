using UnityEngine;
using System.Collections;

public class GuardFlankPursueState : GuardPursueState
{
    public GuardFlankPursueState(NPCStateMachine stateMachine)
        : base(stateMachine)
    {
    }

    public override void Entry()
    {
        base.Entry();
    }

    public override NPCState Update()
    {
        if (false)
        {

        }
        else
        {
            base.Update();
        }

        return this;
    }
}
