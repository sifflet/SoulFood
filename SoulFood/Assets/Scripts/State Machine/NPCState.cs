using UnityEngine;
using System.Collections;

public abstract class NPCState
{
    protected NPCStateMachine stateMachine;

    public NPCState(NPCStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public abstract void Entry();
    public abstract NPCState Update();
}