using UnityEngine;
using System.Collections;

public abstract class NPCStateMachine : MonoBehaviour
{
    protected NPCDriver npc;
    protected NPCState currentState;

    public NPCDriver NPC { get { return this.npc; } }
    public NPCState CurrentState { get { return this.currentState; } }

    public virtual void Setup(NPCDriver npc)
    {
        this.npc = npc;
    }

    public void EnterFirstState()
    {
        if (this.currentState == null) return;
        this.currentState.Entry();
    }

    public abstract void Reset();

    public void Update()
    {
        if (this.currentState == null) return;

        NPCState transitionState = currentState.Update();

        if (currentState != transitionState)
        {
            transitionState.Entry();
            currentState = transitionState;
        }
    }
}
