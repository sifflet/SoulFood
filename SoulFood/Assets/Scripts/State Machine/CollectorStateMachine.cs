﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectorStateMachine : NPCStateMachine
{
	/* Collector variables */
	public const float SOUL_COLLISION_RANGE = 1f;
	public const float TREE_MOVEMENT_COLLISION_RANGE = 0.25f; // Used for NPC to move onto the button
	public const float TREE_COLLISION_RANGE = 2f;			  // Used for NPC to inform caller of tree help call they have arrived at the tree
	public const float TIME_SPENT_SOUL_SEARCHING = 10.0f;
	public const float TIME_SPENT_SINGLE_TREE_SEARCHING = 15.0f;
	public const float TIME_SPENT_MULTIPLE_TREE_SEARCHING = 15.0f;
	public const float TIME_SPENT_WAITING_FOR_TREE_HELP = 15.0f;
	public const float FLEE_RANGE = 20.0f;
	public const float EMERGENCY_FLEE_RANGE = 10.0f;
	public const float SOUL_COLLECTIBLE_RANGE_FOR_STATE_TRIGGER = 2.5f;
	public enum FleeRangeType { Default, Emergency };
	public const float IMMORTALITY_TIME = 8.0f;

	// Variables for handling help calls as caller
	public List<CollectorDriver> collectorsAskedForHelp = new List<CollectorDriver>();		// This a list of Collectors that were asked for help
	private List<CollectorDriver> collectorsArrivedForHelp = new List<CollectorDriver>();		// This a list of Collectors that were asked for help

	// Variables for handling help calls as callee
	private CollectorStateMachine callerStateMachine;
	private bool hasReceivedHelpCall = false;
	private SoulTree targetTree;

	// Variables for handling player help calls as the player
	public List<CollectorDriver> collectorNPCsComingToHelp = new List<CollectorDriver>();
	public bool hasCancelledHelpCallsAsPlayer = false;
	public bool hasReceivedPlayerHelpCall = false;
	
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
		this.hasReceivedPlayerHelpCall = false;
		this.targetTree = null;
		this.callerStateMachine = null;
	}

	// Methods for helping players with trees
	public SoulTree  GetTreeWithPlayersOnThem() {
		Button[] allButtons = GameManager.AllButtons;
		SoulTree treeWithPlayer = null;

		foreach (Button button in allButtons) {
			SoulTree treeForButton = button.GetSoulTreeForCurrentButton();
			if (button.IsTriggered 															// Button is currently triggered
			    && !button.CollectorCurrentlyOnTheButton.ControlledByAI						// A player is stepping on the button
			    && treeForButton.CheckIfTreeHasMultipleButtons() 							// The tree has multiple buttons
			    && treeForButton.IsFull 													// The tree is full
			    && !treeForButton.CheckIfTreeIsCompletelyTargettedOrTriggered())			// The tree has untriggered/targetted buttons
			{
				treeWithPlayer = treeForButton;
				this.callerStateMachine = button.CollectorCurrentlyOnTheButton.StateMachine as CollectorStateMachine;
			}
		}

		return treeWithPlayer;
	}

	public void NotifyPlayerOfHelp(CollectorDriver collector) {
		this.collectorNPCsComingToHelp.Add (collector);
		this.hasCancelledHelpCallsAsPlayer = false;

		Debug.Log (collector.name + ": Player, I'm coming to help!");
		// Sound effect
		AudioClip onMyWayClip = Resources.Load("On_My_Way", typeof(AudioClip)) as AudioClip;
		this.NPC.audioSource.clip = onMyWayClip;
		this.NPC.audioSource.Play();
	}

	// If I'm a player, the moment I step off my button, cancel the help calls of everyone that came to help me
	public bool CheckIfPlayerIsTriggeringTheTree(SoulTree targetTree) {
		if (callerStateMachine)
		{
			CollectorDriver playerDriver = this.callerStateMachine.NPC as CollectorDriver;
			foreach (GameObject buttonObj in targetTree.TreeButtons) {
				Button buttonScript = buttonObj.GetComponent<Button>();
				if (buttonScript.IsTriggered && buttonScript.CollectorCurrentlyOnTheButton == playerDriver)
					return true;
			}
		}
		return false;
	}

	public void CancelHelpCallsAfterPlayerCall() 
	{
		Debug.Log (this.NPC.name + ": Player has left the tree, cancel everything.");
		foreach (CollectorDriver collectorDriver in collectorNPCsComingToHelp) {
			(collectorDriver.StateMachine as CollectorStateMachine).CancelTreeHelpCall();
		}
		CancelTargettingOfTreeButtons();
		collectorNPCsComingToHelp.Clear();
		this.hasCancelledHelpCallsAsPlayer = true;
	}

	private void CancelTargettingOfTreeButtons() 
	{
		Button[] allButtons = GameManager.AllButtons;

		foreach (CollectorDriver collectorDriver in this.collectorNPCsComingToHelp) {
			foreach (Button button in allButtons) {
				if (button.CollectorCurrentlyOnTheButton == collectorDriver)
					button.IsTargettedForTriggering = false;
			}
		}
	}
}
