using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectorStateMachine : NPCStateMachine
{
	/* Collector variables */
	public const float SOUL_COLLISION_RANGE = 1f;
	public const float TREE_MOVEMENT_COLLISION_RANGE = 0.25f; // Used for NPC to move onto the button
	public const float TREE_COLLISION_RANGE = 1f;			  // Used for NPC to inform caller of tree help call they have arrived at the tree
	public const float TIME_SPENT_SOUL_SEARCHING = 10.0f;
	public const float TIME_SPENT_SINGLE_TREE_SEARCHING = 1.0f;
	public const float TIME_SPENT_MULTIPLE_TREE_SEARCHING = 120.0f;
	public const float TIME_SPENT_WAITING_FOR_TREE_HELP = 15.0f;
	public const float FLEE_RANGE = 20.0f;
	public const float EMERGENCY_FLEE_RANGE = 10.0f;
	public const float SOUL_COLLECTIBLE_RANGE_FOR_STATE_TRIGGER = 2.5f;
	public enum FleeRangeType { Default, Emergency };
	public const float IMMORTALITY_TIME = 3.0f;

	// Variables for handling help calls as caller
	public List<CollectorDriver> collectorsAskedForHelp = new List<CollectorDriver>();		// This a list of Collectors that were asked for help
	private List<CollectorDriver> collectorsArrivedForHelp = new List<CollectorDriver>();		// This a list of Collectors that were asked for help

	// Variables for handling help calls as callee
	private CollectorStateMachine callerStateMachine;
	private bool hasReceivedHelpCall = false;
	private SoulTree targetTree;
	
	public bool HasReceivedHelpCall { get { return this.hasReceivedHelpCall; } }
	public SoulTree TargetTree { get { return this.targetTree; } }
	public CollectorStateMachine CallerStateMachine { get { return this.callerStateMachine; } }
	
    public override void Setup(NPCDriver npc)
    {
        base.Setup(npc);
        this.currentState = new CollectorSearchSoulsState(this);
    }

    public override void Reset()
    {
        this.currentState = new CollectorSearchSoulsState(this);
    }

	// Caller methods
	public void NotifyCallerOfHelpArrival(CollectorDriver collectorDriver)
	{
		collectorsArrivedForHelp.Add(collectorDriver);
		Debug.Log (this.NPC.name + " collectorsArrivedForHelp count: " + collectorsArrivedForHelp.Count);
	}

	public bool CheckIfHelpCalleesHaveArrived() 
	{
		if (collectorsArrivedForHelp.Count == 0)
			return false;

		if (collectorsArrivedForHelp.Count == collectorsAskedForHelp.Count) 
			return true;

		return false;
	}

	public void CleanUpCallerAfterCall() 
	{
		collectorsAskedForHelp.Clear ();
		collectorsArrivedForHelp.Clear ();
	}

	// Callee methods
	public void ReceiveTreeHelpCall(SoulTree targetTree, CollectorStateMachine callerStateMachine) 
	{
		Debug.Log (this.NPC.name + " Callee: Got a help call!");
		this.hasReceivedHelpCall = true;
		this.targetTree = targetTree;
		this.callerStateMachine = callerStateMachine;
	}

	public void CancelTreeHelpCall() 
	{
		Debug.Log (this.NPC.name + " Callee: Cancel my help call!");
		this.hasReceivedHelpCall = false;
		this.targetTree = null;
		this.callerStateMachine = null;
	}
}
