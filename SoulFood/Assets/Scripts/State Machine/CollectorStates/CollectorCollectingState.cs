using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectorCollectingState : NPCState
{
    protected List<NPCDriver> guardsInSight;

    public CollectorCollectingState(NPCStateMachine stateMachine)
        : base(stateMachine)
    {
        this.guardsInSight = new List<NPCDriver>();
    }

    public override void Entry()
    {
    }

    public override NPCState Update()
    {
        return this;
    }

}
