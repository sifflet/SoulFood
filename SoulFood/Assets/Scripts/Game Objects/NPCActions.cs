using UnityEngine;
using System.Collections;

public static class NPCActions
{
    public static void ConsumeSoul(CollectorDriver collector)
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
            ConsumeSoul(collector, closestSoul.gameObject);
        };
    }

	public static void ConsumeSoul(CollectorDriver collector, GameObject soul)
	{
		Vector3 collectorPos = collector.Instance.transform.position;
			
		soul.GetComponent<Soul>().IsConsumed(collectorPos);
		collector.AddSoul();
		GameManager.SoulConsumed();
    }

    public static void EjectSoul(CollectorDriver collector, int numberOfSoulsEjected)
    {
        collector.DropSoul(numberOfSoulsEjected);
        GameManager.SoulEjected(numberOfSoulsEjected);
    }

    public static void Lunge()
	{
		
	}
}