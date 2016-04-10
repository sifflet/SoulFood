using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class NPCStateMachine : MonoBehaviour
{
    protected NPCDriver npc;
    protected NPCState currentState;
	protected Stack<NPCState> stateStack;

    public NPCDriver NPC { get { return this.npc; } }
    public NPCState CurrentState { get { return this.currentState; } }

    public virtual void Setup(NPCDriver npc)
    {
        this.npc = npc;
    }

    public virtual void EnterFirstState()
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

    public void ChangeCurrentState(NPCState newState)
    {
        this.currentState = newState;
    }

	public void PushStateOnStack(NPCState state)
	{
		this.stateStack.Push(state);
	}

	public NPCState PopStateOffStack()
	{
		return this.stateStack.Pop();
	}
}
