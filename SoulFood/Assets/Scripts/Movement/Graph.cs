﻿using System;
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
			var smallest = FindNodeWithMinCost(nodes, distances);	// Get smallest distanced node

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
        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();
        Dictionary<Node, Node> previous = new Dictionary<Node, Node>();
        Dictionary<Node, float> costSoFar = new Dictionary<Node, float>();
        Dictionary<Node, float> totalCost = new Dictionary<Node, float>();

        foreach (Node node in vertices.Keys)
        {
            previous[node] = null;
            costSoFar[node] = float.MaxValue;
            totalCost[node] = float.MaxValue;
        }

        openList.Add(start);
        costSoFar[start] = 0.0f;
        totalCost[start] = CalculateEuclideanDistanceHeuristic(start, finish);

        while (openList.Count > 0)
        {
            Node currentNode = FindNodeWithMinCost(openList, totalCost);
            currentNode.SetMaterialColor(Color.blue);

            if (currentNode == finish) return StackToList(GetPath(start, finish, previous));

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (Node neighbor in vertices[currentNode].Keys)
            {
                if (closedList.Contains(neighbor)) continue;

                float newCostSoFar = costSoFar[currentNode] + vertices[currentNode][neighbor];

                if (!openList.Contains(neighbor))
                {
                    openList.Add(neighbor);
                    neighbor.SetMaterialColor(Color.yellow);
                }

                if (newCostSoFar >= costSoFar[neighbor]) continue;

                previous[neighbor] = currentNode;
                costSoFar[neighbor] = newCostSoFar;
                totalCost[neighbor] = costSoFar[neighbor] + CalculateEuclideanDistanceHeuristic(neighbor, finish);
            }
        }

        return null;
	}

	private float CalculateEuclideanDistanceHeuristic(Node currentNode, Node targetNode) {
        return Vector3.Distance(currentNode.position, targetNode.position);
	}

	private Node FindNodeWithMinCost(List<Node> nodeList, Dictionary<Node, float> cost) {
		Node minCostNode = nodeList[0];

		foreach (var node in nodeList) {
            if (cost[node] < cost[minCostNode])
            {
				minCostNode = node;
			}
		}

		return minCostNode;
	}

    private Stack<Node> GetPath(Node start, Node finish, Dictionary<Node, Node> previousNode)
    {
        Stack<Node> path = new Stack<Node>();
        Node currentNode = finish;
        currentNode.SetMaterialColor(Color.red);

        while (previousNode[currentNode] != null)
        {
            path.Push(currentNode);
            currentNode = previousNode[currentNode];
            currentNode.SetMaterialColor(Color.red);
        }

        path.Push(currentNode);

        return path;
    }

    private List<Node> StackToList(Stack<Node> path)
    {
        List<Node> result = new List<Node>();

        while (path.Count > 0)
        {
            result.Add(path.Pop());
        }

        return result;
    }
}
