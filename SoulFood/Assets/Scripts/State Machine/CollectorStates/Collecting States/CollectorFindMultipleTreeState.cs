﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectorFindMultipleTreeState : CollectorCollectingSuperState {
	
	private NPCMovementDriver movementDriver;
	
	public CollectorFindMultipleTreeState(NPCStateMachine stateMachine)
		: base(stateMachine)
	{
	}
	
	public override void Entry()
	{
		Debug.Log (this.stateMachine.NPC.name + ": Find Multiple Tree State Entry");
		movementDriver = this.stateMachine.NPC.MovementDriver;
	}
	
	public override NPCState Update()
	{
		// Check if guards are in sight and if a transition to flee states is necessary
		// These checks are in the base state
		NPCState stateFromBase = base.Update();
		if (stateFromBase != this)
		{
			return stateFromBase;
		}
		
		movementDriver = this.stateMachine.NPC.MovementDriver;

		
		return this;
	}
}
