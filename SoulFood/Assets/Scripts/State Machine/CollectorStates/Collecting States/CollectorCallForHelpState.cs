using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectorCallForHelpState : CollectorCollectingSuperState {

	private float waitForHelpTimer = CollectorStateMachine.TIME_SPENT_WAITING_FOR_TREE_HELP;
	private float delayBeforeCancellingHelpCallsTimer = 2f;
	private NPCMovementDriver movementDriver;
	private SoulTree targetTree;

	private int numOfHelpCallsNeeded = 0;
	private List<GameObject> treeButtonsThatNeedTriggering = new List<GameObject>();
	private List<CollectorDriver> collectorsAskedForHelp = new List<CollectorDriver>();		// This a list of Collectors that were asked for help
	private bool hasAskedForHelp = false;
	private bool hasCancelledHelpCalls = false;
	private bool everyoneIsOnTheTree = false;
	
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

		// Timers
		waitForHelpTimer -= Time.deltaTime;
		if (everyoneIsOnTheTree)
			delayBeforeCancellingHelpCallsTimer -= Time.deltaTime;

		movementDriver = this.stateMachine.NPC.MovementDriver;

		// Make help calls for each button that needs a collector
		if (!this.hasAskedForHelp) {
			MakeHelpCalls();
			Debug.Log (this.stateMachine.NPC.name + " Caller: I've asked for help! collectorsAskedForHelp count: " + collectorsAskedForHelp.Count );
		}

		// If help callees have arrived, cancel the help calls
		if ((this.stateMachine as CollectorStateMachine).CheckIfHelpCalleesHaveArrived() && !this.hasCancelledHelpCalls) {
			everyoneIsOnTheTree = true;
		}

		if (delayBeforeCancellingHelpCallsTimer <= 0 && !this.hasCancelledHelpCalls)	{// If set delay for remaining on the tree passes, cancel everyone's help calls
			Debug.Log (this.stateMachine.NPC.name + " Caller: Tree's been hit, I'm cancelling all help calls and cleaning up");
			CancelHelpCalls();
			(this.stateMachine as CollectorStateMachine).CleanUpCallerAfterCall();
			everyoneIsOnTheTree = false;
		}

		// Return to search soul state if help took too long to show up
		if (waitForHelpTimer <= 0 ) {
			CancelHelpCalls();
			return new CollectorSearchSoulsState(this.stateMachine);
		}

		return this;
	}

	private void GetButtonTriggeringInfoFromTargetTree() 
	{
		List<GameObject> treeButtons = this.targetTree.TreeButtons;
		Debug.Log ("Tree found! Button count: " + treeButtons.Count);
		foreach (GameObject button in treeButtons) {
			Button buttonScript = button.GetComponent<Button>();
			if (!buttonScript.IsTriggered) {
				this.numOfHelpCallsNeeded++;
				this.treeButtonsThatNeedTriggering.Add(button);
			}
		}
		Debug.Log ("Buttons that need triggering: " + treeButtonsThatNeedTriggering.Count);
	}

	private void MakeHelpCalls()
	{
		List<GameObject> availableCollectors = GameManager.Collectors;

		// Remove current collector from availableCollectors list
		availableCollectors.Remove(this.stateMachine.NPC.Instance);

		foreach (GameObject button in treeButtonsThatNeedTriggering) {
			if (availableCollectors.Count >= 1) {	// If there is at least one collector available
				GameObject closestCollector = NPCStateHelper.FindClosestGameObject(this.stateMachine.NPC.Instance, availableCollectors);
				CollectorDriver closestCollectorDriver = closestCollector.GetComponent<CollectorDriver>();
				if (closestCollectorDriver.ControlledByAI) {	// If the closest Collector is a NPC
					(closestCollectorDriver.StateMachine as CollectorStateMachine).ReceiveTreeHelpCall(this.targetTree, this.stateMachine as CollectorStateMachine);
					this.collectorsAskedForHelp.Add (closestCollectorDriver);
				}
				else {	// If the closest Collector is a player
					// TODO: Handle asking player for help via sounds and UI
				}

				// Remove collector that was just called for help from availableCollectors list
				availableCollectors.Remove(closestCollector);
			}
		}
		(this.stateMachine as CollectorStateMachine).collectorsAskedForHelp = this.collectorsAskedForHelp;
		this.hasAskedForHelp = true;
	}

	private void CancelHelpCalls() 
	{
		Debug.Log (this.stateMachine.NPC.name + " Caller: Cancel everyone's help calls! collectorsAskedForHelpCopy count: " + this.collectorsAskedForHelp.Count );
		foreach (CollectorDriver collectorDriver in collectorsAskedForHelp) {
			(collectorDriver.StateMachine as CollectorStateMachine).CancelTreeHelpCall();
		}
		collectorsAskedForHelp.Clear();
		this.hasCancelledHelpCalls = true;
	}	
}
