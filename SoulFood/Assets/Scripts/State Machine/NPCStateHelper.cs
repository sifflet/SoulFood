using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class NPCStateHelper
{
    public static float GetShortestPathDistance(GameObject obj, GameObject target)
    {
        float result = 0.0f;
        Node start = FindClosestNode(obj);
        Node end = FindClosestNode(target);

        List<Node> path = Graph.ShortestPathEuclideanHeuristic(start, end);
        if (path.Count <= 0) return 0.0f;

        result += Vector3.Distance(obj.transform.position, path[0].position);

        for (int i = 0; i < path.Count - 1; ++i)
        {
            result += Vector3.Distance(path[i].position, path[i + 1].position);
        }

        return result;
    }

    /// <summary>
    /// Moves a NPC towards a target, uses the pathfinding algorithm if the target is far
    /// or uses Steering arrive if the target is close.
    /// </summary>
    /// <param name="npc">The moving NPC's NPCDriver</param>
    /// <param name="target">The target game object</param>
    /// <param name="directMoveDistance">The distance at which the NPC should start using
    /// steering arrive to move towards its target</param>
    public static void MoveTo(NPCDriver npc, GameObject target, float directMoveDistance)
    {
        float distanceToTarget = GetShortestPathDistance(npc.Instance, target);

        if (distanceToTarget > directMoveDistance)
        {
            npc.MovementDriver.ChangePath(FindClosestNode(target));
        }
        else
        {
			float npcYPosition = npc.gameObject.transform.position.y;
			Vector3 targetPosition = new Vector3(target.transform.position.x, npcYPosition, target.transform.position.z);
			npc.MovementDriver.NPCMovement.Steering_Arrive(targetPosition, true);
        }
	}

    public static Node FindClosestNode(GameObject obj)
    {
        Node result = null;

        for (int i = 0; i < GameManager.AllNodes.Count; i++)
        {
            if (i == 0)
            {
                result = GameManager.AllNodes[i];
            }
            if (Vector3.Distance(obj.transform.position, GameManager.AllNodes[i].transform.position) < Vector3.Distance(obj.transform.position, result.transform.position))
            {
                result = GameManager.AllNodes[i];
            }
        }

        return result;
    }

	public static GameObject FindClosestGameObject(GameObject mainObj, List<GameObject> gameObjects)
	{
		GameObject result = null;
		
		for (int i = 0; i < gameObjects.Count; i++)
		{
			if (i == 0)
			{
				result = gameObjects[i];
			}
			if (Vector3.Distance(mainObj.transform.position, gameObjects[i].transform.position) < Vector3.Distance(mainObj.transform.position, result.transform.position))
			{
				result = gameObjects[i];
			}
		}
		
		return result;
	}

	public static GameObject FindClosestGameObjectByPath(GameObject mainObj, List<GameObject> gameObjects)
	{
		GameObject result = null;
		
		for (int i = 0; i < gameObjects.Count; i++)
		{
			if (i == 0)
			{
				result = gameObjects[i];
			}
			if (GetShortestPathDistance(mainObj, gameObjects[i]) < GetShortestPathDistance(mainObj, result))
			{
				result = gameObjects[i];
			}
		}
		
		return result;
	}

	public static bool IsColliding(NPCDriver npc, GameObject collisionTarget)
	{
		Collider[] collisions = npc.CollisionArray;

		foreach (Collider collider in collisions) {
			if (collider.gameObject == collisionTarget) {
				return true;
			}
		}

		return false;
	}

	public static bool IsWithinCollisionRangeAtGroundLevel(GameObject mainObj, GameObject collisionObj) 
	{
		Vector3 collisionObjGroundLevelPos = collisionObj.transform.position;
		collisionObjGroundLevelPos.y = 0.0f;
		
		Vector3 mainObjGroundLevelPos = mainObj.transform.position;
		mainObjGroundLevelPos.y = 0.0f;
		
		if (Vector3.Distance(mainObjGroundLevelPos, collisionObjGroundLevelPos) <= GameManager.COLLISION_RANGE)
		{
			return true;
		}

		return false;
	}
}
