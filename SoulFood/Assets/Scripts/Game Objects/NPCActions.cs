using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public static class NPCActions
{
	[Command]
    public static void CmdConsumeSoul(CollectorDriver collector)
    {
		Soul closestSoul = null;
		Vector3 collectorPos = collector.Instance.transform.position;
		Collider[] collisions = collector.CollisionArray;
		
		for (int i = 0; i < collector.CollisionArray.Length; ++i)
		{
			Soul soul = collisions[i].GetComponent<Soul>();
			if (soul && Mathf.Abs((collectorPos - collisions[i].transform.position).magnitude) <= GameManager.COLLISION_RANGE)
			{
				closestSoul = soul;
			}
		}

        if (closestSoul)
		{
            CmdConsumeSoul(collector, closestSoul.gameObject);
        }
    }

	[Command]
	public static void CmdConsumeSoul(CollectorDriver collector, GameObject soul)
	{
		Vector3 collectorPos = collector.Instance.transform.position;
			
		soul.GetComponent<Soul>().IsConsumed(collectorPos);
		collector.AddSoul();
		collector.MovementDriver.RecalculateSpeedBasedOnSoulConsumption();
		collector.isCollectingSoul = true;	//Triggers sound effect
		GameObject.FindGameObjectWithTag("GameController").SendMessage("SoulConsumed");
    }

	[Command]
    public static void CmdEjectSoul(CollectorDriver collector, int numberOfSoulsEjected)
    {
        collector.DropSoul(numberOfSoulsEjected);
		collector.MovementDriver.RecalculateSpeedBasedOnSoulConsumption();
		collector.isDroppingSoul = true;	//Triggers sound effect
        GameObject.FindGameObjectWithTag("GameController").SendMessage("SoulEjected",numberOfSoulsEjected);
    }

	// Should be used through the GuardLungeState only, as it implements a required timer
	[Command]
	public static void CmdLunge(NPCDriver guard, Vector3 direction)
	{
        if (guard.ControlledByAI)
        {
            guard.Instance.GetComponent<Rigidbody>().AddForce(direction * 100f * Time.deltaTime, ForceMode.Impulse);
        }
        else
        {
            // dont know why we need to increase the force here
            guard.Instance.GetComponent<Rigidbody>().AddForce(direction * 1000f * Time.deltaTime, ForceMode.Impulse);
        }
    }
}