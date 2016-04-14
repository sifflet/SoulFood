using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCMovementDriver : MonoBehaviour
{
	private NPCMovement currentNPC;

	// Used for calculation of soul speed adjustment
	private float minimumSpeed = 3f; 	
	private int hypotheticalMaximumSoulCapacity = 40;

	// Pathfinding variables
	private Node startNode, endNode;
	private List<Node> pathList;
    private List<Node> nodes;

    public bool AttainedFinalNode { get; set; }

    void Start()
    {
    }

    public Node CurrentTargetNode
    { 
        get
        {
            if (this.pathList.Count == 0) return null;

            int index = pathCounter;
            if (pathCounter > pathList.Count - 1) index = pathList.Count - 1;
            return this.pathList[index];
        }
    }

    public NPCMovement NPCMovement { get { return this.currentNPC; } }
	
	public void Setup (NPCMovement movement)
    {
        nodes = GameManager.AllNodes;
        this.currentNPC = movement;
        this.pathList = new List<Node>();
        this.AttainedFinalNode = false;
	}
	
	private int pathCounter = 0;		// Used for keeping track where NPC is along a path
	// Update is called once per frame
	public void Update () {
		/**
		 * Move NPC along a path
		 */
		if (pathList.Count > pathCounter && endNode == pathList[pathList.Count - 1])
		{
            Vector3 target = pathList[pathCounter].transform.position;
            target.y = currentNPC.transform.position.y;

			//endNodeIndicator.transform.position = endNode.transform.position;
			if (Vector3.Angle (currentNPC.transform.forward, (target - currentNPC.transform.position)) > 35)
			{
				currentNPC.Steering_Stop ();
				currentNPC.rotateTowards (target);
			}
			else
            {
				if (pathCounter == pathList.Count - 1) 
				{
                    currentNPC.Steering_Arrive(target, true);
				}
				else 
				{
                    currentNPC.Steering_Arrive(target, false);
				}
			}

			bool nodeAttained = false;
			Collider[] collisionArray = Physics.OverlapSphere (currentNPC.transform.position, 2f);
			for (int i = 0; i < collisionArray.Length; i++)
            {
				if (collisionArray[i].GetComponent (typeof(Node)) == pathList[pathCounter])
                {
					nodeAttained = true;
				}
			}
			
			if (nodeAttained)
            {
                if (this.CurrentTargetNode == this.endNode) this.AttainedFinalNode = true;
				pathCounter++;
			}
		}
	}

    public void ChangePath(Node endNode)
    {
        this.startNode = FindClosestNode();
        this.endNode = endNode;
        this.pathList = Graph.ShortestPathEuclideanHeuristic(startNode, endNode);
        this.AttainedFinalNode = false;
        this.pathCounter = 0;
    }

    public void ChangePathToFlee(float terminateDistance, List<NPCDriver> threats)
    {
        this.startNode = FindClosestNode();
        this.pathList = Graph.InverseAStar(startNode, terminateDistance, threats);
        if (pathList.Count > 0)
        {
            this.endNode = pathList[pathList.Count - 1];
        }
        else
        {
            this.endNode = startNode;
        }
        this.AttainedFinalNode = false;
        this.pathCounter = 0;
    }

    public void ChangePathToFlank(Node endNode, NPCDriver otherGuard)
    {
        this.startNode = FindClosestNode();
        this.endNode = endNode;
        this.pathList = Graph.AStarFlank(startNode, endNode, otherGuard);
        this.AttainedFinalNode = false;
        this.pathCounter = 0;
    }

    public Node FindClosestNode()
    {
        Node result = null;

        for (int i = 0; i < nodes.Count; i++)
        {
            if (i == 0)
            {
                result = nodes[i];
            }
            if (Vector3.Distance(currentNPC.transform.position, nodes[i].transform.position) < Vector3.Distance(currentNPC.transform.position, result.transform.position))
            {
                result = nodes[i];
            }
        }

        return result;
    }

	/**
	 * Methods for speed adjustment based on number of consumed souls
	 */	
	public void RecalculateSpeedBasedOnSoulConsumption() 
	{
		CollectorDriver collectorDriver = this.currentNPC.gameObject.GetComponent<CollectorDriver>();
		float speedDeboost = 0; // Variable is brought out of the if for printing purposes
		currentNPC.MaxSpeed = collectorDriver.MaxSpeed;	// Reset to max speed
		//float startSpeed = currentNPC.MaxSpeed; // For printing purposes
		if (collectorDriver) {
			float speedDecreaseFactor = (currentNPC.MaxSpeed - this.minimumSpeed) / this.hypotheticalMaximumSoulCapacity;
			speedDeboost = speedDecreaseFactor * collectorDriver.SoulsStored;
			if (currentNPC.MaxSpeed - speedDeboost >= this.minimumSpeed)
				currentNPC.MaxSpeed -= speedDeboost;
			else
				currentNPC.MaxSpeed = this.minimumSpeed;
		}
		/*
		Debug.Log (collectorDriver.name + ": Souls -> " + collectorDriver.SoulsStored 
		           + ". Deboost -> " + speedDeboost
		           + ". Start Speed -> " + startSpeed
		           + ". Current Speed -> " + currentNPC.MaxSpeed);
		*/
	}

	/**
	 * Methods for pathfinding graph
	 */	
	private float GetShortestPathDistance(List<List<Node>> paths) {
		float shortestDistance = float.MaxValue;
		foreach (var path in paths) {
			float pathDistance = CalculatePathDistance(path);
			if (pathDistance < shortestDistance) {
				shortestDistance = pathDistance;
			}
		}
		return shortestDistance;
	}
	
	private float CalculatePathDistance(List<Node> path) {
		float distance = 0;
		for (int i = 0; i < path.Count; i++) {
			if (i + 1 < path.Count) {	
				Node currentNode = path[i];
				Node nextNode = path[i + 1];
				if (currentNode.neighboringNodeDistances.ContainsKey(nextNode)) {
					distance += currentNode.neighboringNodeDistances[nextNode];
				}
			}
		}
		return distance;
	}
}
