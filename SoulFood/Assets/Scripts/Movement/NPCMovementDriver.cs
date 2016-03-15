using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCMovementDriver
{
	private NPCMovement currentNPC;

	// Pathfinding variables
	private Node startNode, endNode;
	private List<Node> pathList;
	private Graph graph;
	private List<Node> nodes;

    public Node CurrentTargetNode
    { 
        get
        {
            if (this.pathList.Count == 0) return null;
            return this.pathList[pathCounter];
        }
    }
    public bool AttainedFinalNode { get; set; }
    public List<Node> AllNodes { get { return this.nodes; } }
	
	public NPCMovementDriver (NPCMovement movement)
    {
        this.currentNPC = movement;
        this.pathList = new List<Node>();
		/**
		 * Setup pathfinding graph
		 */
		// Get all node GameObjects in the scene
		GameObject[] nodeGameObjects = GameObject.FindGameObjectsWithTag("Node");

		// Get all Node objects from node GameObjects
		nodes = new List<Node>();
		
		foreach (var nodeGameObject in nodeGameObjects)
        {
			nodes.Add(nodeGameObject.GetComponent<Node>());
		}
		
		// Initialize the pathfinding graph
		graph = new Graph(); 
		
		// Add all nodes to the graph
		foreach (var node in nodes)
        {
			graph.AddVertex(node, node.neighboringNodeDistances);
		}

        this.AttainedFinalNode = false;

		//Set up for NPC movement for testing purposes
        /*
		FindStartNode();
		endNode	= nodes[56]; // End node is hardcoded here for testing purposes
		pathList = graph.ShortestPathEuclideanHeuristic(startNode, endNode);
         * */

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
			Collider[] collisionArray = Physics.OverlapSphere (currentNPC.transform.position, 2.0f);
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
        this.pathList = graph.ShortestPathEuclideanHeuristic(startNode, endNode);
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
