using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectorCollectingState : NPCState
{
    protected List<NPCDriver> guardsInSight;

    public CollectorCollectingState(NPCStateMachine stateMachine)
        : base(stateMachine)
    {
        this.guardsInSight = new List<NPCDriver>();
    }

    public override void Entry()
    {
    }

    public override NPCState Update()
    {
        this.guardsInSight = FindGuardsInSight();

        if (GuardsInEmergencyFleeRange()) ; // return emergencyFlee state
        if (GuardsInFleeRange()) return new CollectorFleeState(this.stateMachine); // return flee state

        return this.stateMachine.CurrentState;
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

    protected bool GuardsInFleeRange()
    {
        Vector3 thisNPCPosition = this.stateMachine.NPC.Instance.transform.position;

        foreach (NPCDriver guard in guardsInSight)
        {
            if (Vector3.Distance(thisNPCPosition, guard.Instance.transform.position) <= (this.stateMachine as CollectorStateMachine).FleeRange)
            {
                return true;
            }
        }

        return false;
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
