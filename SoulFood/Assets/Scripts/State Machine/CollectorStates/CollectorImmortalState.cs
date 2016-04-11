using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectorImmortalState : NPCState
{
    private List<NPCDriver> guardsInSight;
    private float immortalTimer = 0f;

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
		this.stateMachine.NPC.MovementDriver.NPCMovement.MaxSpeed += 5f;
		// Set collector opacity to half
		SetCollectorOpacity(0.5f);
    }

    public override NPCState Update()
    {
        immortalTimer -= Time.deltaTime;

		if (immortalTimer <= 0) {
			// Upon exit of state, remove collector speed boost
			this.stateMachine.NPC.MovementDriver.NPCMovement.MaxSpeed = (this.stateMachine.NPC as CollectorDriver).MaxSpeed;
			// Set collector opacity to full
			SetCollectorOpacity(1f);

			return this.ResetStackToDefaultState(new CollectorSearchSoulsState(this.stateMachine));
		}

        if (this.stateMachine.NPC.MovementDriver.AttainedFinalNode)
        {
            this.stateMachine.NPC.MovementDriver.ChangePathToFlee(CollectorStateMachine.FLEE_RANGE, guardsInSight);
        }

        return this;
    }

	private void SetCollectorOpacity(float alpha)
	{
		Transform knightTransform = this.stateMachine.NPC.Instance.transform.Find ("Knight");
		Material knightMaterial = knightTransform.gameObject.GetComponent<SkinnedMeshRenderer>().material;
		//foreach (Material material in knightMaterials) {
		Color color = knightMaterial.color;
		color.a = alpha;
		knightMaterial.color = color;
		//}
	}
}
