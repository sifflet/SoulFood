using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public static class CollectorStateHelper {

	public static void GetNewRandomPath(NPCMovementDriver movementDriver) 
	{
		if (movementDriver.AttainedFinalNode)
		{
			Node newEndNode = GameManager.AllNodes[UnityEngine.Random.Range(0, GameManager.AllNodes.Count - 1)];
			movementDriver.ChangePath(newEndNode);
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

	// Note: Can't seem to make this method generic using System.Type as a parameter
	// Thus, the repetition here
	public static List<GameObject> FindVisibleTrees(NPCDriver npc)
	{
		List<GameObject> visibleTrees = new List<GameObject>();
		SoulTree[] allTrees = GameObject.FindObjectsOfType(typeof(SoulTree)) as SoulTree[];
		
		foreach (SoulTree tree in allTrees)
		{		
			Vector3 viewPortPosition = npc.CameraDriver.Camera.WorldToViewportPoint(tree.gameObject.transform.position);
			
			if (viewPortPosition.x >= 0.0f && viewPortPosition.x <= 1.0f &&
			    viewPortPosition.y >= 0.0f && viewPortPosition.y <= 1.0f &&
			    viewPortPosition.z >= 0.0f)
			{
				visibleTrees.Add(tree.gameObject);
			}
		}
		
		return visibleTrees;
	}


	public static GameObject FindClosestFullTreeButton(NPCDriver npc, int treeType) 
	{
		List<GameObject> visibleTrees = FindVisibleTrees(npc);
		List<SoulTree> filteredTrees = new List<SoulTree>();
		List<GameObject> filteredTreeObjects = new List<GameObject>();

		// Filter tree list by desired tree type and whether or not it has souls
		foreach (GameObject tree in visibleTrees) 
		{
			SoulTree treeScript = tree.GetComponent<SoulTree>();
			if (treeScript.TreeType == treeType && treeScript.IsFull)
			{
				filteredTrees.Add(treeScript);
				filteredTreeObjects.Add(tree);
			}
		}

		if (filteredTrees.Count > 1)
		{
			GameObject closestTreeObj = NPCStateHelper.FindClosestGameObjectByPath(npc.gameObject, filteredTreeObjects);
			// Get closest button
			GameObject firstButtonOfTree = closestTreeObj.GetComponent<SoulTree>().TreeButtons[0];
			if (firstButtonOfTree)
				return firstButtonOfTree;
		}
		else if  (filteredTrees.Count == 1) {
			GameObject firstButtonOfTree = filteredTrees[0].TreeButtons[0];
			if (firstButtonOfTree)
				return firstButtonOfTree;
		}
		return null;
	}
}
