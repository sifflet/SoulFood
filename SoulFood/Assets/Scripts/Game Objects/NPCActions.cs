﻿using UnityEngine;
using System.Collections;

public static class NPCActions
{
    public static void ConsumeSoul(CollectorDriver collector)
    {
		Soul closestSoul = null;
		float closestDistance = 0.5f; //adjust size upon implementation
		Vector3 collectorPos = collector.Instance.transform.position;
		Collider[] collisions = collector.CollisionArray;
		
		for (int i = 0; i < collector.CollisionArray.Length; i++)
		{
			Soul soul = collisions[i].GetComponent<Soul>();
			if (soul && Mathf.Abs((collectorPos - collisions[i].transform.position).magnitude) <= closestDistance)
			{
				closestSoul = soul;
			}
		}
		
		if (closestSoul)
		{
			closestSoul.IsConsumed(collectorPos);
			collector.AddSoul();
			GameManager.SoulConsumed();
			Debug.Log("I've consumed a soul!"); //to be removed
		};
    }

	public static void ConsumeSoul(CollectorDriver collector, GameObject soul)
	{
		Vector3 collectorPos = collector.Instance.transform.position;
			
		soul.GetComponent<Soul>().IsConsumed(collectorPos);
		collector.AddSoul();
		GameManager.SoulConsumed();
	}


    public static void EjectSoul()
    {

    }

	public static void Lunge()
	{
		
	}

}
