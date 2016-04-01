﻿using UnityEngine;
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

    public static void MoveTo(NPCDriver npc, GameObject target, float directMoveDistance)
    {
        float distanceToTarget = GetShortestPathDistance(npc.Instance, target);

        if (distanceToTarget > directMoveDistance)
        {
            npc.MovementDriver.ChangePath(FindClosestNode(target));
        }
        else
        {
            npc.MovementDriver.NPCMovement.Steering_Arrive(target.transform.position, false);
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
}
