using UnityEngine;
using System.Collections;

public abstract class GuardPursueState : NPCState
{
    protected NPCDriver otherGuard;
    protected float pursueNewTargetTimer = 0f;

    public GuardPursueState(NPCStateMachine stateMachine)
        : base(stateMachine)
    {
    }

    public override void Entry()
    {
        NPCMovementDriver thisNPCMovementDriver = this.stateMachine.NPC.MovementDriver;
        GuardDriver guardDriver = this.stateMachine.NPC as GuardDriver;
        this.otherGuard = FindOtherGuard();

        if (guardDriver.IsLeader)
        {
            (stateMachine as GuardStateMachine).TargetNPC = GetClosestVisibleCollector();

            NPCState otherGuardTransitionState = new GuardFlankPursueState(otherGuard.StateMachine);
            otherGuardTransitionState.Entry();
            (otherGuard.StateMachine as GuardStateMachine).ChangeCurrentState(otherGuardTransitionState);
        }
        else
        {
            (stateMachine as GuardStateMachine).TargetNPC = (otherGuard.StateMachine as GuardStateMachine).TargetNPC;
        }

        NPCStateHelper.MoveTo(stateMachine.NPC, (stateMachine as GuardStateMachine).TargetNPC.Instance, 1f);
    }

    public override NPCState Update()
    {
        if (NPCStateHelper.GetShortestPathDistance(stateMachine.NPC.Instance, (stateMachine as GuardStateMachine).TargetNPC.Instance) <= GameManager.ACTIVATE_LUNGE_DISTANCE &&
            Vector3.Distance(stateMachine.NPC.Instance.transform.position, (stateMachine as GuardStateMachine).TargetNPC.Instance.transform.position) <= GameManager.ACTIVATE_LUNGE_DISTANCE &&
            TargetInLungeCone())
        {
            return new GuardLungeState(stateMachine);
        }

        if (stateMachine.NPC.VisibleNPCs.Count == 0) return new GuardSearchState(stateMachine);

        // if a collector passes close by
        NPCDriver newTarget = GetCollectorInLungeRange();
        if (newTarget != null)
        {
            pursueNewTargetTimer = GameManager.PURSUE_NEW_TARGET_TIME;
            (stateMachine as GuardStateMachine).TargetNPC = newTarget;
        }

        if (!(stateMachine.NPC as GuardDriver).IsLeader && pursueNewTargetTimer > 0)
        {
            pursueNewTargetTimer -= Time.deltaTime;

            if (pursueNewTargetTimer <= 0)
            {
                (stateMachine as GuardStateMachine).TargetNPC = (otherGuard.StateMachine as GuardStateMachine).TargetNPC;
                pursueNewTargetTimer = 0.0f;
            }
        }

        return this.stateMachine.CurrentState;
    }

    protected NPCDriver GetClosestVisibleCollector()
    {
        NPCDriver result = this.stateMachine.NPC.VisibleNPCs[0];
        float currentClosestDistance = NPCStateHelper.GetShortestPathDistance(stateMachine.NPC.Instance, stateMachine.NPC.VisibleNPCs[0].Instance);

        foreach (NPCDriver npc in this.stateMachine.NPC.VisibleNPCs)
        {
            GameObject thisNPC = this.stateMachine.NPC.Instance;
            GameObject otherNPC = npc.Instance;
            GameObject currentClosestNPC = result.Instance;

            float distanceToOtherNPC = NPCStateHelper.GetShortestPathDistance(thisNPC, otherNPC);
            if (distanceToOtherNPC < currentClosestDistance)
            {
                result = npc;
                currentClosestDistance = distanceToOtherNPC;
            }
        }

        return result;
    }

    protected NPCDriver FindOtherGuard()
    {
        foreach (NPCDriver npc in GameManager.Guards)
        {
            if (npc == this.stateMachine.NPC) continue;

            return npc;
        }

        return null;
    }

    protected bool TargetInLungeCone()
    {
        Vector3 targetDir = (stateMachine as GuardStateMachine).TargetNPC.Instance.transform.position - stateMachine.NPC.Instance.transform.position;
        Vector3 forward = stateMachine.NPC.Instance.transform.forward;

        return Vector3.Angle(targetDir, forward) < GameManager.LUNGE_CONE_ANGLE;
    }

    protected NPCDriver GetCollectorInLungeRange()
    {
        foreach (NPCDriver npc in GameManager.Deathies)
        {
            if (npc == (stateMachine as GuardStateMachine).TargetNPC) continue;
            if ((npc as CollectorDriver).IsImmortal) continue;
            if (NPCStateHelper.IsWithinCollisionRangeAtGroundLevel(stateMachine.NPC.Instance, npc.Instance, GameManager.LUNGE_COLLISION_RANGE)) return npc;
        }

        return null;
    }
}
