using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectorSearchSoulsState : CollectorCollectingSuperState
{
	private float soulSearchingTimer = GameManager.TIME_SPENT_SOUL_SEARCHING;
	private List<GameObject> visibleSouls = new List<GameObject>();

	private NPCMovementDriver movementDriver;

    public CollectorSearchSoulsState(NPCStateMachine stateMachine)
        : base(stateMachine)
    {
    }

    public override void Entry()
    {
        movementDriver = this.stateMachine.NPC.MovementDriver;
        Node newEndNode = GameManager.AllNodes[UnityEngine.Random.Range(0, GameManager.AllNodes.Count - 1)];
        movementDriver.ChangePath(newEndNode);
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

		soulSearchingTimer -= Time.deltaTime;
        movementDriver = this.stateMachine.NPC.MovementDriver;

		if (soulSearchingTimer > 0) {	// If the time period for soul searching is not over

			// Collect souls in close proximity as you go
			FindVisibleSouls();
			if (visibleSouls.Count > 0) {
				GameObject closetSoul = NPCStateHelper.FindClosestGameObject(this.stateMachine.NPC.gameObject, visibleSouls);
				Debug.Log ("In search state: Moving to " + closetSoul.name + " position x " + closetSoul.transform.position.x + "at position z " + closetSoul.transform.position.z);
				NPCStateHelper.MoveTo(this.stateMachine.NPC, closetSoul, 5f);
				// To add? Stop movement when in collider range?
				return new CollectorCollectSoulsState(this.stateMachine); // Transition to CollectSouls state
			
			}

			// If we're at the end of our path, find a new random one
	        if (movementDriver.AttainedFinalNode)
	        {
	            Node newEndNode = GameManager.AllNodes[UnityEngine.Random.Range(0, GameManager.AllNodes.Count - 1)];
	            movementDriver.ChangePath(newEndNode);
	        }
		}
		else {	// Time period for soul searching is over
			// Transition to looking for soul-tree with help call 
		}

        return this;
    }

	private void FindVisibleSouls()
	{
		this.visibleSouls.Clear();
		
		Soul[] allSouls = GameObject.FindObjectsOfType(typeof(Soul)) as Soul[];

		foreach (Soul soul in allSouls)
		{		
			Vector3 viewPortPosition = this.stateMachine.NPC.CameraDriver.Camera.WorldToViewportPoint(this.stateMachine.NPC.Instance.transform.position);
			
			if (viewPortPosition.x >= 0.0f && viewPortPosition.x <= 1.0f &&
			    viewPortPosition.y >= 0.0f && viewPortPosition.y <= 1.0f &&
			    viewPortPosition.z >= 0.0f)
			{
				visibleSouls.Add(soul.gameObject);
			}
		}

		// Sort visible souls by path distance, closest to farthest
		/*visibleSouls.Sort (delegate(Soul x, Soul y) 
		{
			float xDistance = NPCStateHelper.GetShortestPathDistance(this.stateMachine.NPC.gameObject, x.gameObject);
			float yDistance = NPCStateHelper.GetShortestPathDistance(this.stateMachine.NPC.gameObject, y.gameObject);

			if (xDistance == yDistance) return 0;
			else if (xDistance < yDistance) return -1;
			else if (xDistance > yDistance) return 1;
			else return 0;
		});*/
		
	}
}
