using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : MonoBehaviour {
	
	public Vector3 position;
	public Dictionary<Node, float> neighboringNodeDistances;

	// Variables used to control to scan around the node for other nodes
	private float angle = 0;	
	private Vector3 direction;
	private RaycastHit hit;

	private ArrayList nodePositions;

	// Use this for initialization
	void Awake () {
		nodePositions = new ArrayList();
		neighboringNodeDistances = new Dictionary<Node, float>();
		position = transform.position;

		// Locate neighboring nodes for this node
		while (angle <2*Mathf.PI) {
			// Update raycast direction
			direction = new Vector3(Mathf.Cos((angle)), 0, Mathf.Sin((angle)));
			
			// Create raycast
			Vector3 fwd = transform.TransformDirection(direction);
			if (Physics.Raycast(transform.position, fwd, out hit, 8)) {
				Transform targetTransform = hit.transform;
				if (targetTransform.tag == "Node"){
					Vector3 forward = targetTransform.position - transform.position;
					nodePositions.Add(forward);

					// Add neighbor and distances
					neighboringNodeDistances[hit.transform.GetComponent<Node>()] = Vector3.Distance(targetTransform.position, transform.position);
				}
			}
			angle = (angle + 0.05f);
		}
	}
	
	// Update is called once per frame
	void Update () {
		// Display edges
		foreach (var nodePos in nodePositions) {
			Debug.DrawRay(transform.position,(Vector3) nodePos, Color.grey);
		}
	}

	public void SetMaterialColor(Color color) {
		Renderer nodeRenderer = transform.GetComponent<Renderer>();
		nodeRenderer.material.color = color;
	}
}
