using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectorFleeState : NPCState
{
    protected List<NPCDriver> guardsInSight;

    public CollectorFleeState(NPCStateMachine stateMachine)
        : base(stateMachine)
    {
    }

    public override void Entry()
    {
        Debug.Log("Flee State Entry");
        this.guardsInSight = FindGuardsInSight();
        List<NPCDriver> guardsInFleeRange = FindGuardsInFleeRange();
        this.stateMachine.NPC.MovementDriver.ChangePathToFlee((this.stateMachine as CollectorStateMachine).FleeRange, guardsInFleeRange);
    }

    public override NPCState Update()
    {
        this.guardsInSight = FindGuardsInSight();
        List<NPCDriver> guardsInFleeRange = FindGuardsInFleeRange();

        if (guardsInSight.Count == 0) return new CollectorSearchSoulsState(this.stateMachine);
        if (guardsInFleeRange.Count == 0) return new CollectorSearchSoulsState(this.stateMachine);
        if (GuardsInEmergencyFleeRange()) ; // return emergency flee state

        if (this.stateMachine.NPC.MovementDriver.AttainedFinalNode)
        {
            this.stateMachine.NPC.MovementDriver.ChangePathToFlee((this.stateMachine as CollectorStateMachine).FleeRange, guardsInFleeRange);
        }

        return this;
    }

    protected List<NPCDriver> FindGuardsInSight()
    {
        List<NPCDriver> result = new List<NPCDriver>();

        foreach (NPCDriver guard in GameManager.Guards)
        {
            if (this.stateMachine.NPC.VisibleNPCs.Contains(guard))
            {
                result.Add(guard);
            }
        }

        return result;
    }

    protected List<NPCDriver> FindGuardsInFleeRange()
    {
        List<NPCDriver> result = new List<NPCDriver>();
        Vector3 thisNPCPosition = this.stateMachine.NPC.Instance.transform.position;

        foreach (NPCDriver guard in guardsInSight)
        {
            if (Vector3.Distance(thisNPCPosition, guard.Instance.transform.position) <= (this.stateMachine as CollectorStateMachine).FleeRange)
            {
                result.Add(guard);
            }
        }

        return result;
    }

    protected bool GuardsInEmergencyFleeRange()
    {
        Vector3 thisNPCPosition = this.stateMachine.NPC.Instance.transform.position;

        foreach (NPCDriver guard in guardsInSight)
        {
            if (Vector3.Distance(thisNPCPosition, guard.Instance.transform.position) <= (this.stateMachine as CollectorStateMachine).EmergencyFleeRange)
            {
                return true;
            }
        }

        return false;
    }
}
