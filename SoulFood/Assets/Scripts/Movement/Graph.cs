using System;
using System.Collections.Generic;
using UnityEngine;


class Graph
{
	// Collection with all the nodes and neighbors of nodes
	Dictionary<Node, Dictionary<Node, float>> vertices = new Dictionary<Node, Dictionary<Node, float>>();
	

	// AddVertex is used to add nodes to the Graph's vertices
	public void AddVertex(Node node, Dictionary<Node, float> edges)
	{
		vertices[node] = edges;
	}
	

	public List<Node> ShortestPathNullHeuristic(Node start, Node finish)
	{
		var previous = new Dictionary<Node, Node>();	// Previous nodes in shortest path
		var distances = new Dictionary<Node, float>();	// Distances from source to current node
		var nodes = new List<Node>();	// All nodes in Q
		
		Stack<Node> pathStack = null;	
		List<Node> pathList = new List<Node>(); // Shortest path to return
		
		foreach (var vertex in vertices)
		{
			if (vertex.Key == start)
			{
				distances[vertex.Key] = 0;	// Distance from source to source
			}
			else
			{
				distances[vertex.Key] = float.MaxValue;	// Unknown distance from source for current node
			}
			
			nodes.Add(vertex.Key);	// Add node to list containing all nodes
		}
		
		while (nodes.Count != 0)	// While nodes list is not empty
		{
			var smallest = FindNodeWithMinDistanceFromGivenNode(nodes, distances);	// Get smallest distanced node

			nodes.Remove(smallest);		// Remove smallest distanced node from all nodes list

			// Set color of node to blue to indicate that the node has been visited
			//smallest.SetMaterialColor(Color.blue);

			if (smallest == finish)		// If the smallest distanced node is the target
			{
				// Start tracing the shortest path
				pathStack = new Stack<Node>();	
				while (previous.ContainsKey(smallest))
				{
					pathStack.Push(smallest);
					// Set color of node to red to indicate that it's part of the shortest path
					//smallest.SetMaterialColor(Color.red);

					smallest = previous[smallest];
				}
				break;
			}

			foreach (var neighbor in vertices[smallest])
			{
				//neighbor.Key.SetMaterialColor(Color.green);
				var alt = distances[smallest] + neighbor.Value;
				if (alt < distances[neighbor.Key])	// Found a shorter path!
				{
					distances[neighbor.Key] = alt;
					previous[neighbor.Key] = smallest;
				}
			}
		}

		// Cache path stack into list
		while (pathStack.Count > 0) {
			pathList.Add (pathStack.Pop());
		}
		
		return pathList;
	}

	public List<Node> ShortestPathEuclideanHeuristic(Node start, Node finish)
	{
		var previous = new Dictionary<Node, Node>();	// Previous nodes in shortest path
		var distances = new Dictionary<Node, float>();	// Distances from source to current node
		var nodes = new List<Node>();	// All nodes in Q
		
		Stack<Node> pathStack = null;
		List<Node> pathList = new List<Node>(); // Shortest path to return

		foreach (var vertex in vertices)
		{
			if (vertex.Key == start)
			{
				distances[vertex.Key] = 0;	// Distance from source to source
			}
			else
			{
				distances[vertex.Key] = float.MaxValue;	// Unknown distance from source for current node
			}
			
			nodes.Add(vertex.Key);	// Add node to list containing all nodes
		}
		
		while (nodes.Count != 0)	// While nodes list is not empty
		{
			var smallest = FindNodeWithMinDistanceFromGivenNode(nodes, distances);	// Get smallest distanced node
			
			nodes.Remove(smallest);		// Remove smallest distanced node from all nodes list
			
			// Set color of node to blue to indicate that the node has been visited
			smallest.SetMaterialColor(Color.blue);
			
			if (smallest == finish)		// If the smallest distanced node is the target
			{
				// Start tracing the shortest path
				pathStack = new Stack<Node>();	
				while (previous.ContainsKey(smallest))
				{
					pathStack.Push(smallest);
					// Set color of node to red to indicate that it's part of the shortest path
					smallest.SetMaterialColor(Color.red);
					
					smallest = previous[smallest];
				}

				break;
			}
			
			foreach (var neighbor in vertices[smallest])
			{
				//neighbor.Key.SetMaterialColor(Color.green);
				var alt = distances[smallest] + neighbor.Value 
					+ CalculateEuclideanDistanceHeuristic(neighbor.Key, finish);
				if (alt < distances[neighbor.Key])	// Found a shorter path!
				{
					distances[neighbor.Key] = alt;
					previous[neighbor.Key] = smallest;
				}
			}
		}
		
		// Cache path stack into list
		while (pathStack.Count > 0) {
			pathList.Add (pathStack.Pop());
		}
		
		return pathList;
	}

	private float CalculateEuclideanDistanceHeuristic(Node currentNode, Node targetNode) {
		float dx = Math.Abs(currentNode.position.x - targetNode.position.x);
		float dz = Math.Abs(currentNode.position.z - targetNode.position.z);
		return (float)Math.Sqrt((dx * dx) + (dz * dz));
	}

	private Node FindNodeWithMinDistanceFromGivenNode(List<Node> nodeList, Dictionary<Node, float> distances) {
		Node minDistanceNode = nodeList[0];

		foreach (var node in nodeList) {
			if (distances[node] < distances[minDistanceNode]) {
				minDistanceNode = node;
			}
		}

		return minDistanceNode;
	}

}
