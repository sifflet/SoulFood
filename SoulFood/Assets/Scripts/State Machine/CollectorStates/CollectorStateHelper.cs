﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public static class CollectorStateHelper {

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
			if (NPCStateHelper.GetShortestPathDistance(thisNPC, guard.Instance) <= (npcStateMachine as CollectorStateMachine).FleeRange)
			{
				result.Add(guard);
			}
		}
		
		return result;
	}


	public static bool GuardsInFleeRange(NPCStateMachine npcStateMachine, GameManager.FleeRangeType range) // Range should be "default" for default flee range or "emergency" for emergency flee range
	{
        GameObject thisNPC = npcStateMachine.NPC.Instance;
		float fleeRange = (npcStateMachine as CollectorStateMachine).FleeRange; // Default flee range
		List<NPCDriver> guardsInSight = FindGuardsInSight(npcStateMachine);

		// Get emergency range based on inputted string
		if (range == GameManager.FleeRangeType.Emergency)
		{
			fleeRange = (npcStateMachine as CollectorStateMachine).EmergencyFleeRange;
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

	public static List<GameObject> FindVisibleSouls(NPCDriver npc)
	{
		List<GameObject> visibleSouls = new List<GameObject>();
		Soul[] allSouls = GameObject.FindObjectsOfType(typeof(Soul)) as Soul[];
		
		foreach (Soul soul in allSouls)
		{		
			Vector3 viewPortPosition = npc.CameraDriver.Camera.WorldToViewportPoint(npc.Instance.transform.position);
			
			if (viewPortPosition.x >= 0.0f && viewPortPosition.x <= 1.0f &&
			    viewPortPosition.y >= 0.0f && viewPortPosition.y <= 1.0f &&
			    viewPortPosition.z >= 0.0f)
			{
				visibleSouls.Add(soul.gameObject);
			}
		}

		return visibleSouls;
	}
}
