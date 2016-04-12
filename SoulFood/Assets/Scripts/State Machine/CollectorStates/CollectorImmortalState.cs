using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectorImmortalState : NPCState
{
    private List<NPCDriver> guardsInSight;
    private float immortalTimer = 0f;
	private Material transparentMaterial = Resources.Load("Transparent", typeof(Material)) as Material;
	private Material[] knightMaterials = new Material[2];
	private Material swordMaterial;

    public CollectorImmortalState(NPCStateMachine stateMachine)
        : base(stateMachine)
    {
    }

    public override void Entry()
    {
		// Add state on to stack
		this.stateMachine.PushStateOnStack(this);

        Debug.Log(this.stateMachine.NPC.name + ": Immortal state entry");
        this.immortalTimer = CollectorStateMachine.IMMORTALITY_TIME;
        this.guardsInSight = CollectorStateHelper.FindGuardsInSight(this.stateMachine);
        this.stateMachine.NPC.MovementDriver.ChangePathToFlee(CollectorStateMachine.FLEE_RANGE, guardsInSight);

		// Upon entry, give collector a speed boost
		this.stateMachine.NPC.MovementDriver.NPCMovement.MaxSpeed += 8f;
		// Set collector opacity to transparent
		SetCollectorOpacity();
    }

    public override NPCState Update()
    {
        immortalTimer -= Time.deltaTime;

		if (immortalTimer <= 0) {
			// Upon exit of state, remove collector speed boost
			this.stateMachine.NPC.MovementDriver.NPCMovement.MaxSpeed = (this.stateMachine.NPC as CollectorDriver).MaxSpeed;
			// Set collector opacity to full
			RemoveCollectorOpacity();

			return this.ResetStackToDefaultState(new CollectorSearchSoulsState(this.stateMachine));
		}

        if (this.stateMachine.NPC.MovementDriver.AttainedFinalNode)
        {
			if (guardsInSight.Count > 0)
            	this.stateMachine.NPC.MovementDriver.ChangePathToFlee(CollectorStateMachine.FLEE_RANGE, guardsInSight);
			else
				CollectorStateHelper.GetNewPath(this.stateMachine.NPC.MovementDriver, GameManager.AllNodes[UnityEngine.Random.Range(0, GameManager.AllNodes.Count - 1)]); 
        }

        return this;
    }

	private void SetCollectorOpacity()
	{
		Transform knightTransform = this.stateMachine.NPC.Instance.transform.GetChild(0).GetChild(0);
		knightMaterials = knightTransform.gameObject.GetComponent<Renderer>().materials;
		Material[] transparentMaterials = new Material[2];
		transparentMaterials[0] = transparentMaterial;
		transparentMaterials[1] = transparentMaterial;
		knightTransform.gameObject.GetComponent<Renderer>().materials = transparentMaterials;

		Transform swordTransform = this.stateMachine.NPC.Instance.transform.GetChild(0).GetChild(4);
		swordMaterial = swordTransform.gameObject.GetComponent<Renderer>().material;
		swordTransform.gameObject.GetComponent<Renderer>().material = transparentMaterial;
	}

	private void RemoveCollectorOpacity()
	{
		Transform knightTransform = this.stateMachine.NPC.Instance.transform.GetChild(0).GetChild(0);
		knightTransform.gameObject.GetComponent<Renderer>().materials = knightMaterials;

		Transform swordTransform = this.stateMachine.NPC.Instance.transform.GetChild(0).GetChild(4);
		swordTransform.gameObject.GetComponent<Renderer>().material = swordMaterial;
	}
}
