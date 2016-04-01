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
		Vector3 thisNPCPosition = npcStateMachine.NPC.Instance.transform.position;
		List<NPCDriver> guardsInSight = FindGuardsInSight(npcStateMachine);


		foreach (NPCDriver guard in guardsInSight)
		{
			if (Vector3.Distance(thisNPCPosition, guard.Instance.transform.position) <= (npcStateMachine as CollectorStateMachine).FleeRange)
			{
				result.Add(guard);
			}
		}
		
		return result;
	}


	public static bool GuardsInFleeRange(NPCStateMachine npcStateMachine, string range) // Range should be "default" for default flee range or "emergency" for emergency flee range
	{
		Vector3 thisNPCPosition = npcStateMachine.NPC.Instance.transform.position;
		float fleeRange = (npcStateMachine as CollectorStateMachine).FleeRange; // Default flee range
		List<NPCDriver> guardsInSight = FindGuardsInSight(npcStateMachine);

		// Get emergency range based on inputted string
		if (range.Equals("emergency", StringComparison.OrdinalIgnoreCase))
		{
			fleeRange = (npcStateMachine as CollectorStateMachine).EmergencyFleeRange;
		}


		foreach (NPCDriver guard in guardsInSight)
		{
			if (Vector3.Distance(thisNPCPosition, guard.Instance.transform.position) <= fleeRange)
			{
				return true;
			}
		}
		
		return false;
	}
	
}
