using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public static class CollectorStateHelper {

	private const float DELAY_IN_DROPPING_SOULS = 0.5f;
	
	public static void GetNewPath(NPCMovementDriver movementDriver, Node endNode) 
	{
		if (movementDriver.AttainedFinalNode)
		{
			movementDriver.ChangePath(endNode);
		}
	}

	public static List<NPCDriver> FindGuardsInSight(NPCStateMachine npcStateMachine)
	{
		List<NPCDriver> result = new List<NPCDriver>();
		
		foreach (NPCDriver guard in GameManager.Guards)
		{
			if (npcStateMachine.NPC.VisibleNPCs.Contains(guard))
			{
				result.Add(guard);
			}
		}
		
		return result;
	}
	
	public static List<NPCDriver> FindGuardsInFleeRange(NPCStateMachine npcStateMachine)
	{
		List<NPCDriver> result = new List<NPCDriver>();
		GameObject thisNPC = npcStateMachine.NPC.Instance;
		List<NPCDriver> guardsInSight = FindGuardsInSight(npcStateMachine);


		foreach (NPCDriver guard in guardsInSight)
		{
			if (NPCStateHelper.GetShortestPathDistance(thisNPC, guard.Instance) <= CollectorStateMachine.FLEE_RANGE)
			{
				result.Add(guard);
			}
		}
		
		return result;
	}


	public static bool GuardsInFleeRange(NPCStateMachine npcStateMachine, CollectorStateMachine.FleeRangeType range) // Range should be "default" for default flee range or "emergency" for emergency flee range
	{
        GameObject thisNPC = npcStateMachine.NPC.Instance;
		float fleeRange = CollectorStateMachine.FLEE_RANGE; // Default flee range
		List<NPCDriver> guardsInSight = FindGuardsInSight(npcStateMachine);

		// Get emergency range based on inputted string
		if (range == CollectorStateMachine.FleeRangeType.Emergency)
		{
			fleeRange = CollectorStateMachine.EMERGENCY_FLEE_RANGE;
		}


		foreach (NPCDriver guard in guardsInSight)
		{
			if (NPCStateHelper.GetShortestPathDistance(thisNPC, guard.Instance) <= fleeRange)
			{
				return true;
			}
		}
		
		return false;
	}

	public static bool SoulsInCollectibleRange(NPCStateMachine npcStateMachine)
	{
		GameObject thisNPC = npcStateMachine.NPC.Instance;
		float collectibleRange = CollectorStateMachine.SOUL_COLLECTIBLE_RANGE_FOR_STATE_TRIGGER;
		List<GameObject> soulsInSight = FindVisibleSouls(npcStateMachine.NPC);

		Vector3 npcGroundLevelPos = thisNPC.transform.position;
		npcGroundLevelPos.y = 0.0f;

		foreach (GameObject soul in soulsInSight)
		{
			Vector3 soulGroundLevelPos = soul.transform.position;
			soulGroundLevelPos.y = 0.0f;

			if (Vector3.Distance(npcGroundLevelPos, soulGroundLevelPos) <= collectibleRange)
			{
				return true;
			}
		}
		
		return false;
	}

	public static List<GameObject> FindVisibleSouls(NPCDriver npc)
	{
		List<GameObject> visibleSouls = new List<GameObject>();
		Soul[] allSouls = GameObject.FindObjectsOfType(typeof(Soul)) as Soul[];
		
		foreach (Soul soul in allSouls)
		{		
			Vector3 viewPortPosition = npc.CameraDriver.Camera.WorldToViewportPoint(soul.gameObject.transform.position);
			
			if (viewPortPosition.x >= 0.0f && viewPortPosition.x <= 1.0f &&
			    viewPortPosition.y >= 0.0f && viewPortPosition.y <= 1.0f &&
			    viewPortPosition.z >= 0.0f)
			{
				visibleSouls.Add(soul.gameObject);
			}
		}

		return visibleSouls;
	}

	public static bool HasVisibleSouls(NPCDriver npc) 
	{
		List<GameObject> visibleSouls = FindVisibleSouls(npc);

		if (visibleSouls.Count > 0)
			return true;

		return false;
	}
    
	public static GameObject FindClosestFullTreeButton(NPCDriver npc, int treeType) 
	{
		List<GameObject> visibleTrees = NPCStateHelper.FindVisibleTrees(npc);
		List<SoulTree> filteredTrees = new List<SoulTree>();
		List<GameObject> filteredTreeObjects = new List<GameObject>();
		SoulTree targetTree = null;	// Default just to assign variable
		GameObject targetbuttonObject;

		// Filter tree list by desired tree type and whether or not it has souls
		foreach (GameObject tree in visibleTrees) 
		{
			SoulTree treeScript = tree.GetComponent<SoulTree>();
			if (treeScript.TreeType == treeType && treeScript.IsFull && !treeScript.CheckIfTreeIsTargetted())
			{
				filteredTrees.Add(treeScript);
				filteredTreeObjects.Add(tree);
			}
		}
		// If there is more than one tree, find the closest tree
		if (filteredTrees.Count > 1)
		{
			GameObject closestTreeObj = NPCStateHelper.FindClosestGameObjectByPath(npc.gameObject, filteredTreeObjects);
			targetTree = closestTreeObj.GetComponent<SoulTree>();
		}
		else if  (filteredTrees.Count == 1) { 	// There is only one tree
			targetTree = filteredTrees[0];
		}

		// If the tree is multibutton, get closest button of tree
		if (targetTree) {
			List<GameObject> treeButtons = targetTree.TreeButtons;
			if (treeButtons.Count > 1) {
				GameObject closestButtonOfTree = NPCStateHelper.FindClosestGameObject(npc.gameObject, treeButtons);
				if (closestButtonOfTree)
					return closestButtonOfTree;
			}
			else {	// Tree has only one button, return it
				return targetTree.TreeButtons[0];
			}
		}

		return null;
	}

	public static Node FindNodeForRememberedTreePosition(NPCStateMachine npcStateMachine)
	{
		GameObject treePosition = null;

		if (npcStateMachine.TreesFound.Count > 0) {
			int randomIndex = UnityEngine.Random.Range(0, npcStateMachine.TreesFound.Count - 1);
			treePosition = npcStateMachine.TreesFound[0];
		}

		if (treePosition == null)
			return GameManager.AllNodes[UnityEngine.Random.Range(0, GameManager.AllNodes.Count - 1)];

		NPCStateHelper.FindClosestNode(treePosition.GetComponent<SoulTree>().TreeButtons[0]).gameObject.GetComponent<MeshRenderer>().enabled = true;
		return NPCStateHelper.FindClosestNode(treePosition.GetComponent<SoulTree>().TreeButtons[0]);
	}

	public static void DropSouls(CollectorDriver collectorDriver, int numSoulsToDrop)
	{
		float delayInDroppingSoulsTimer = DELAY_IN_DROPPING_SOULS;
		while (numSoulsToDrop > 0) {
			delayInDroppingSoulsTimer -= Time.deltaTime;
			
			if (delayInDroppingSoulsTimer <= 0) {
				collectorDriver.DropSoul(1);
				delayInDroppingSoulsTimer = DELAY_IN_DROPPING_SOULS;
			}
			
			numSoulsToDrop--;
		}
	}
}
