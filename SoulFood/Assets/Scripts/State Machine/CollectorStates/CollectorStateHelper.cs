using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class CollectorStateHelper {

	public static List<NPCDriver> FindGuardsInSight(NPCState npcState)
	{
		List<NPCDriver> result = new List<NPCDriver>();
		
		foreach (NPCDriver guard in GameManager.Guards)
		{
			if (npcState.stateMachine.NPC.VisibleNPCs.Contains(guard))
			{
				result.Add(guard);
			}
		}
		
		return result;
	}
	
	public static List<NPCDriver> FindGuardsInFleeRange(NPCState npcState)
	{
		List<NPCDriver> result = new List<NPCDriver>();
		Vector3 thisNPCPosition = npcState.stateMachine.NPC.Instance.transform.position;
		
		foreach (NPCDriver guard in guardsInSight)
		{
			if (Vector3.Distance(thisNPCPosition, guard.Instance.transform.position) <= (npcState.stateMachine as CollectorStateMachine).FleeRange)
			{
				result.Add(guard);
			}
		}
		
		return result;
	}


	public static bool GuardsInFleeRange(NPCState npcState, string range) // Range should be "default" for default flee range or "emergency" for emergency flee range
	{
		Vector3 thisNPCPosition = npcState.stateMachine.NPC.Instance.transform.position;
		float fleeRange;

		// Get range based on inputted string
		if (range.Equals("default", StringComparison.OrdinalIgnoreCase))
		{
			fleeRange = (npcState.stateMachine as CollectorStateMachine).FleeRange;
		}
		else if (range.Equals("emergency", StringComparison.OrdinalIgnoreCase))
		{
			fleeRange = (npcState.stateMachine as CollectorStateMachine).EmergencyFleeRange;
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

	/*

	public static bool GuardsInFleeRange(NPCState npcState) 
	{
		Vector3 thisNPCPosition = npcState.stateMachine.NPC.Instance.transform.position;
		
		foreach (NPCDriver guard in guardsInSight)
		{
			if (Vector3.Distance(thisNPCPosition, guard.Instance.transform.position) <= (npcState.stateMachine as CollectorStateMachine).FleeRange)
			{
				return true;
			}
		}
		
		return false;
	}

	public static bool GuardsInEmergencyFleeRange(NPCState npcState)
	{
		Vector3 thisNPCPosition = npcState.stateMachine.NPC.Instance.transform.position;
		
		foreach (NPCDriver guard in guardsInSight)
		{
			if (Vector3.Distance(thisNPCPosition, guard.Instance.transform.position) <= (npcState.stateMachine as CollectorStateMachine).EmergencyFleeRange)
			{
				return true;
			}
		}
		
		return false;
	}*/
}
