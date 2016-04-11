﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class CollectorCollectingSuperState : NPCState
{
    protected List<NPCDriver> guardsInSight;

    public CollectorCollectingSuperState(NPCStateMachine stateMachine)
        : base(stateMachine)
    {
		// Add state on to stack
		this.stateMachine.PushStateOnStack(this);

		this.guardsInSight = new List<NPCDriver>();
    }

    public override void Entry()
    {
	}

    public override NPCState Update()
    {
		this.guardsInSight = CollectorStateHelper.FindGuardsInSight(this.stateMachine);

		// If you're close to a guard, you need to move to flee state (quick change)
		if (CollectorStateHelper.GuardsInFleeRange(this.stateMachine, CollectorStateMachine.FleeRangeType.Emergency)) ; // return emergencyFlee state
		if (CollectorStateHelper.GuardsInFleeRange(this.stateMachine, CollectorStateMachine.FleeRangeType.Default)) return new CollectorFleeState(this.stateMachine); // return flee state

		// If you're near a soul, pick it up!
		if (CollectorStateHelper.SoulsInCollectibleRange(this.stateMachine) && stateMachine.CurrentState.GetType() != typeof(CollectorCollectSoulsState)) 
			return new CollectorCollectSoulsState(this.stateMachine); // return soul collecting state

		// If you receive a help call while not collecting souls, answer it!
		if ((this.stateMachine as CollectorStateMachine).HasReceivedHelpCall 
		    && stateMachine.CurrentState.GetType() != typeof(CollectorAnswerHelpCallState)
		    && stateMachine.CurrentState.GetType() != typeof(CollectorCollectSoulsState)) 
			return new CollectorAnswerHelpCallState(this.stateMachine, 
			                                        (this.stateMachine as CollectorStateMachine).TargetTree, 
			                                        (this.stateMachine as CollectorStateMachine).CallerStateMachine);

		// If you receive a help call while collecting souls, insert the answer help call state as the previous state in the stack
		// This is done so that you can finish collecting souls and then transition to your previous state which is now answering the help call
		if ((this.stateMachine as CollectorStateMachine).HasReceivedHelpCall && stateMachine.CurrentState.GetType() == typeof(CollectorCollectSoulsState)) 
			this.InsertStateAsPreviousStateInStack(new CollectorAnswerHelpCallState(this.stateMachine, 
			                                                                        (this.stateMachine as CollectorStateMachine).TargetTree, 
			                                                                        (this.stateMachine as CollectorStateMachine).CallerStateMachine));

		// If you receive a help call while calling for help on a multitree, clear your stack history and then answer the call
		if ((this.stateMachine as CollectorStateMachine).HasReceivedHelpCall 
		    && stateMachine.CurrentState.GetType() == typeof(CollectorCallForHelpState)) 
		{
			this.ResetStackToDefaultState(new CollectorSearchSoulsState(this.stateMachine));
			return new CollectorAnswerHelpCallState(this.stateMachine, 
			                                        (this.stateMachine as CollectorStateMachine).TargetTree, 
			                                        (this.stateMachine as CollectorStateMachine).CallerStateMachine);
		}

		return this.stateMachine.CurrentState;
    }

}
