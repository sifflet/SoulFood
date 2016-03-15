using UnityEngine;
using System.Collections;

public abstract class NPCStateMachine
{
    protected NPCDriver npc;
    protected NPCState currentState;

    public NPCDriver NPC { get { return this.npc; } }
    public NPCState CurrentState { get { return this.currentState; } }

    protected NPCStateMachine(NPCDriver npc)
    {
        this.npc = npc;
    }

    public void Setup()
    {
        if (this.currentState == null) return;
        this.currentState.Entry();
    }

    public abstract void Reset();

    public void Update()
    {
        if (this.currentState == null) return;
        currentState = currentState.Update();
    }
}
