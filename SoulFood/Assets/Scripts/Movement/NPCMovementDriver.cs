using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCMovementDriver : MonoBehaviour {
	
	public NPCMovement currentNPC;

	// Pathfinding variables
	private Node startNode, endNode;
	private List<Node> pathList;
	private Graph graph;
	private List<Node> nodes;
	

	// Use this for initialization
	void Start () {
		/**
		 * Setup pathfinding graph
		 */
		// Get all node GameObjects in the scene
		GameObject[] nodeGameObjects = GameObject.FindGameObjectsWithTag("Node");

		// Get all Node objects from node GameObjects
		nodes = new List<Node>();
		
		foreach (var nodeGameObject in nodeGameObjects) {			
			nodes.Add(nodeGameObject.GetComponent<Node>());
		}
		
		// Initialize the pathfinding graph
		graph = new Graph(); 
		
		// Add all nodes to the graph
		foreach (var node in nodes) {
			graph.AddVertex(node, node.neighboringNodeDistances);
		}

		//Set up for NPC movement for testing purposes
		FindStartNode();
		endNode	= nodes[56]; // End node is hardcoded here for testing purposes
		pathList = graph.ShortestPathEuclideanHeuristic(startNode, endNode);

	}
	
	private int pathCounter = 0;		// Used for keeping track where NPC is along a path
	// Update is called once per frame
	void Update () {
		/**
		 * Move NPC along a path
		 */
		if (pathList.Count > pathCounter && endNode == pathList[pathList.Count - 1])
		{
			
			//endNodeIndicator.transform.position = endNode.transform.position;
			if (Vector3.Angle (currentNPC.transform.forward, (pathList[pathCounter].transform.position - currentNPC.transform.position)) > 35)
			{
				currentNPC.Steering_Stop ();
				currentNPC.rotateTowards (pathList[pathCounter].transform.position);
			}
			else {
				if (pathCounter == pathList.Count - 1) 
				{
					currentNPC.Steering_Arrive (pathList[pathCounter].transform.position, true);
				}
				else 
				{
					currentNPC.Steering_Arrive (pathList[pathCounter].transform.position, false);
				}
			}
			bool nodeAttained = false;
			Collider[] collisionArray = Physics.OverlapSphere (currentNPC.transform.position, 0.2f);
			for (int i = 0; i < collisionArray.Length; i++) {
				if (collisionArray[i].GetComponent (typeof(Node)) == pathList[pathCounter]) {
					nodeAttained = true;
				}
			}
			
			if (nodeAttained) {
				pathCounter++;
			}
		}

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

	/**
	 * Methods for NPC movement along path
	 */
	//Find the start node according to the position of the NPC
	private void FindStartNode() {
		for (int i = 0; i < nodes.Count; i++) {
			if (i == 0) {
				startNode = nodes[i];
			}
			if ((currentNPC.transform.position - nodes[i].transform.position).magnitude < (currentNPC.transform.position - startNode.transform.position).magnitude) {
				startNode = nodes[i];
			}
		}
	}

}
