﻿using UnityEngine;
using System.Collections;

public static class NPCActions
{
    public static void ConsumeSoul(CollectorDriver collector)
    {
        Soul closestSoul = null;
        float closestDistance = 2f; //adjust size upon implementation
        Vector3 collectorPos = collector.Instance.transform.position;
        Collider[] collisions = collector.CollisionArray;

        for (int i = 0; i < collector.CollisionArray.Length; i++)
        {
            if (collisions[i].tag == "Soul" && Mathf.Abs((collectorPos - collisions[i].transform.position).magnitude) <= closestDistance)
            {
                closestSoul = collisions[i].GetComponent<Soul>();
            }
        }

        if (closestSoul != null)
        {
            closestSoul.getConsumed(collectorPos);
            collector.AddSoul();
            GameManager.SoulConsumed();
            collector.eatingDelay = 0.5f;
            Debug.Log("test"); //to be removed
        };
    }

    public static void EjectSoul()
    {

    }

	public static void Lunge()
	{
		
	}

}
