using UnityEngine;
using System.Collections;

public class CollectorStateMachine : NPCStateMachine
{
	/* Collector variables */
	public const float SOUL_COLLISION_RANGE = 1f;
	public const float TREE_COLLISION_RANGE = 0.25f;
	public const float TIME_SPENT_SOUL_SEARCHING = 10.0f;
	public const float TIME_SPENT_SINGLE_TREE_SEARCHING = 15.0f;
	public const float TIME_SPENT_MULTIPLE_TREE_SEARCHING = 15.0f;
	public const float TIME_SPENT_WAITING_FOR_TREE_HELP = 15.0f;
	public const float FLEE_RANGE = 20.0f;
	public const float EMERGENCY_FLEE_RANGE = 10.0f;
	public const float SOUL_COLLECTIBLE_RANGE_FOR_STATE_TRIGGER = 2.5f;
	public enum FleeRangeType { Default, Emergency };
	public const float IMMORTALITY_TIME = 3.0f;

	// Variables for handling help calls
	private bool hasReceivedHelpCall = false;
	private SoulTree targetTree;
	
	public bool HasReceivedHelpCall { get { return this.hasReceivedHelpCall; } }
	public SoulTree TargetTree { get { return this.targetTree; } }
	
    public override void Setup(NPCDriver npc)
    {
        base.Setup(npc);
        this.currentState = new CollectorSearchSoulsState(this);
    }

    public override void Reset()
    {
        this.currentState = new CollectorSearchSoulsState(this);
    }

	public void ReceiveTreeHelpCall(SoulTree targetTree) 
	{
		this.hasReceivedHelpCall = true;
		this.targetTree = targetTree;
	}

	public void CancelTreeHelpCall() 
	{
		this.hasReceivedHelpCall = false;
		this.targetTree = null;
	}
}
