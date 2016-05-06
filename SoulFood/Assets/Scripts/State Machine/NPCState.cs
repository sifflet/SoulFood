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

	public NPCState GetPreviousStateInStack() 
	{
		NPCState currentState = this.stateMachine.PopStateOffStack();

		if (currentState != this)
			return new CollectorSearchSoulsState(this.stateMachine);	// Default state returned in case something went wrong

		return this.stateMachine.PeekAtTopStateInStack();
	}

	public NPCState ResetStackToDefaultState(NPCState defaultState) 
	{
		this.stateMachine.ResetStackToDefaultState(defaultState);

		return this.stateMachine.PeekAtTopStateInStack();
	}

	public void InsertStateAsPreviousStateInStack(NPCState insertedState)
	{
		this.stateMachine.InsertStateAsPreviousStateInStack(insertedState);
	}
}