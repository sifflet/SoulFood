using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuardSearchState : NPCState
{
    public GuardSearchState(NPCStateMachine stateMachine)
        : base(stateMachine)
    {
    }

    public override void Entry()
    {
		Debug.Log (this.stateMachine.NPC.name + ": Guard Search entry");
        NPCMovementDriver movementDriver = this.stateMachine.NPC.MovementDriver;
        Node newEndNode = GameManager.AllNodes[UnityEngine.Random.Range(0, GameManager.AllNodes.Count - 1)];
        movementDriver.ChangePath(newEndNode);
    }

    public override NPCState Update()
    {
        if (this.stateMachine.NPC.VisibleNPCs.Count > 0) return new GuardFlankPursueState(this.stateMachine);

        NPCMovementDriver movementDriver = this.stateMachine.NPC.MovementDriver;

        if (movementDriver.AttainedFinalNode)
        {
            Node newEndNode = ChooseStrategicPosition();
            movementDriver.ChangePath(newEndNode);
        }

        this.stateMachine.AddVisibleTrees(NPCStateHelper.FindVisibleTrees(stateMachine.NPC));

        return this;
    }
	/*
    protected void AddVisibleTrees(List<GameObject> newTrees)
    {
        foreach (GameObject tree in newTrees)
        {
            if (!(stateMachine as GuardStateMachine).TreesFound.Contains(tree))
            {
                (stateMachine as GuardStateMachine).TreesFound.Add(tree);
            }
        }
    }
    */

    protected Node ChooseStrategicPosition()
    {
        GameObject result = null;
        GuardStateMachine otherGuardFSM = (stateMachine as GuardStateMachine).OtherGuard.StateMachine as GuardStateMachine;

        foreach (GameObject tree in (stateMachine as GuardStateMachine).TreesFound)
        {
            if (otherGuardFSM.StrategicSoulTreeTarget == tree) continue;
            if (result == null)
            {
                result = tree;
                continue;
            }

            if (tree.GetComponent<SoulTree>().TreeType > result.GetComponent<SoulTree>().TreeType)
            {
                result = tree;
            }
        }

        if (result == null) return GameManager.AllNodes[UnityEngine.Random.Range(0, GameManager.AllNodes.Count - 1)];
        return NPCStateHelper.FindClosestNode(result);
    }
}
