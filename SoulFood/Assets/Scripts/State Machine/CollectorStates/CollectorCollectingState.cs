using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectorCollectingState : NPCState
{
    protected float fleeRange = 8.0f;
    protected float emergencyFleeRange = 4.0f;

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

    protected bool GuardInFleeRange()
    {
        Vector3 thisNPCPosition = this.stateMachine.NPC.Instance.transform.position;

        foreach (NPCDriver guard in guardsInSight)
        {
            if (Vector3.Distance(thisNPCPosition, guard.Instance.transform.position) <= fleeRange)
            {
                return true;
            }
        }

        return false;
    }

    protected bool GuardInEmergencyFleeRange()
    {
        Vector3 thisNPCPosition = this.stateMachine.NPC.Instance.transform.position;

        foreach (NPCDriver guard in guardsInSight)
        {
            if (Vector3.Distance(thisNPCPosition, guard.Instance.transform.position) <= emergencyFleeRange)
            {
                return true;
            }
        }

        return false;
    }
}
