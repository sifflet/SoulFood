﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class NPCStateMachine : MonoBehaviour
{
    protected NPCDriver npc;
    protected NPCState currentState;
	protected Stack<NPCState> stateStack;
	
	public List<GameObject> TreesFound { get; set; }
    public GameObject StrategicSoulTreeTarget { get; set; }

    public NPCDriver NPC { get { return this.npc; } }
    public NPCState CurrentState { get { return this.currentState; } }

    public virtual void Setup(NPCDriver npc)
    {
        this.npc = npc;
		this.stateStack = new Stack<NPCState>();
		this.TreesFound = new List<GameObject>();
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

	public NPCState PeekAtTopStateInStack()
	{
		return this.stateStack.Peek();
	}

	public void InsertStateAsPreviousStateInStack(NPCState insertedState)
	{
		NPCState currentState = this.stateStack.Pop();
		this.stateStack.Push(insertedState);
		this.stateStack.Push (currentState);
	}

	public void ResetStackToDefaultState(NPCState defaultState) 
	{
		this.stateStack.Clear();
		this.stateStack.Push(defaultState);
	}

	public string PrintStackContents() 
	{
		NPCState[] statesInStack = this.stateStack.ToArray();
		string output = "";

		for (int i = statesInStack.Length - 1; i >= 0; i--) {
			output += " " + statesInStack[i].GetType();
		}

		return output;
	}	

	public void AddVisibleTrees(List<GameObject> newTrees)
	{
		foreach (GameObject tree in newTrees)
		{
			if (!this.TreesFound.Contains(tree))
			{
				this.TreesFound.Add(tree);
			}
		}
	}
}
