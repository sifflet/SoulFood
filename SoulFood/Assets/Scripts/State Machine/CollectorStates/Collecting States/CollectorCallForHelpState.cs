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

		// Add more wait delay to a triple tree
		if (targetTree.TreeButtons.Count == 3) {
			waitForHelpTimer += 2f;
		}
	}
	
	public override NPCState Update()
	{
		// Check if guards are in sight and if a transition to flee states is necessary
		// These checks are in the base state
		NPCState stateFromBase = base.Update();
		if (stateFromBase != this)
		{
			// Cancell help calls before transitioning out of this state
			if (!this.hasCancelledHelpCalls) 
				CancelHelpCalls();

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

			// Since it is likely souls will now be released from the tree and caller is transitioned into collect soul state
			// We reset the stack history so that after the collect soul state, the caller returns to search for souls and not call for help again
			this.ResetStackToDefaultState(new CollectorSearchSoulsState(this.stateMachine));
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
			return this.ResetStackToDefaultState(new CollectorSearchSoulsState(this.stateMachine));
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
	}

	private void MakeHelpCalls()
	{
		List<NPCDriver> availableCollectors = new List<NPCDriver>(GameManager.Collectors);
		// Remove current collector from availableCollectors list
		availableCollectors.Remove(this.stateMachine.NPC);
		Debug.Log ("Number of buttons that need triggering: " + treeButtonsThatNeedTriggering.Count);
		Debug.Log ("Here is the available collector count: " + availableCollectors.Count);

        foreach (GameObject button in treeButtonsThatNeedTriggering) {
			if (availableCollectors.Count >= 1) {	// If there is at least one collector available
				CollectorDriver closestCollector = NPCStateHelper.FindClosestNPC(this.stateMachine.NPC, availableCollectors) as CollectorDriver;
				if (closestCollector.ControlledByAI) {	// If the closest Collector is a NPC
					(closestCollector.StateMachine as CollectorStateMachine).ReceiveTreeHelpCall(this.targetTree, this.stateMachine as CollectorStateMachine);
					this.collectorsAskedForHelp.Add (closestCollector);
                }
                else {  // If the closest Collector is a player
                        // TODO: Handle asking player for help via sounds and UI

				}

                //HeadsUpDisplay.CreateIndicator(Camera.current.WorldToViewportPoint(this.targetTree.transform.position));
                HeadsUpDisplay.CreateIndicator(new Vector2(0, 125));

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
		CancelTargettingOfTreeButtons(); 
		this.hasCancelledHelpCalls = true;
	}

	private void CancelTargettingOfTreeButtons() 
	{
		foreach (GameObject buttonObj in this.targetTree.TreeButtons) {
			Button buttonScript = buttonObj.GetComponent<Button>();
			buttonScript.IsTargettedForTriggering = false;
		}
	}
}
