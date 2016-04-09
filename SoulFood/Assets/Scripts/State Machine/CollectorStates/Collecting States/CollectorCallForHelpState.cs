using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectorCallForHelpState : CollectorCollectingSuperState {
	
	private NPCMovementDriver movementDriver;
	private SoulTree targetTree;

	private int numOfHelpCallsNeeded = 0;
	private List<GameObject> treeButtonsThatNeedTriggering = new List<GameObject>();
	private bool hasAskedForHelp = false;
	
	public CollectorCallForHelpState(NPCStateMachine stateMachine, SoulTree targetTree)
		: base(stateMachine)
	{
		this.targetTree = targetTree;
	}
	
	public override void Entry()
	{
		Debug.Log (this.stateMachine.NPC.name + ": Call For Help State Entry");
		movementDriver = this.stateMachine.NPC.MovementDriver;
		GetButtonTriggeringInfoFromTargetTree();
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

		// Make help calls for each button that needs a collector
		if (!this.hasAskedForHelp) {
			MakeHelpCalls();
		}
		
		return this;
	}

	private void GetButtonTriggeringInfoFromTargetTree() 
	{
		List<GameObject> treeButtons = this.targetTree.TreeButtons;
		foreach (GameObject button in treeButtons) {
			Button buttonScript = button.GetComponent<Button>();
			if (!buttonScript.IsTriggered) {
				this.numOfHelpCallsNeeded++;
				this.treeButtonsThatNeedTriggering.Add(button);
			}
		}
	}

	private void MakeHelpCalls()
	{
		List<GameObject> availableCollectors = GameManager.Collectors;

		// Remove current collector from availableCollectors list
		availableCollectors.Remove(this.stateMachine.NPC.Instance);

		foreach (GameObject button in treeButtonsThatNeedTriggering) {
			GameObject closestCollector = NPCStateHelper.FindClosestGameObject(this.stateMachine.NPC.Instance, availableCollectors);
			CollectorDriver closestCollectorDriver = closestCollector.GetComponent<CollectorDriver>();
			if (closestCollectorDriver.ControlledByAI) {	// If the closest Collector is a NPC
				(closestCollectorDriver.StateMachine as CollectorStateMachine).ReceiveTreeHelpCall(this.targetTree);
			}
			else {	// If the closest Collector is a player
				// TODO: Handle asking player for help via sounds and UI
			}

			// Remove collector that was just called for help from availableCollectors list
			availableCollectors.Remove(closestCollector);
		}
		
		this.hasAskedForHelp = true;
	}
}
