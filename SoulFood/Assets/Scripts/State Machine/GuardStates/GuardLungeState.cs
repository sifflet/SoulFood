using UnityEngine;
using System.Collections;

public class GuardLungeState : NPCState
{
    public GuardLungeState(NPCStateMachine stateMachine)
        : base(stateMachine)
    {
    }
	
	private float lungeTimer = 0f;		// time on lunging
    private Vector3 lungeDirection;

    public override void Entry()
    {
		Debug.Log (this.stateMachine.NPC.name + ": Lunge entry");

        this.stateMachine.NPC.MovementDriver.enabled = false;
        this.stateMachine.NPC.MovementDriver.NPCMovement.Reset();
		lungeTimer = GuardStateMachine.LUNGE_TIME;
        lungeDirection = this.stateMachine.NPC.Instance.transform.forward;
    }

    public override NPCState Update()
    {
        lungeTimer -= Time.deltaTime;

        if (lungeTimer > 0)
        {
            NPCActions.CmdLunge(this.stateMachine.NPC, lungeDirection);

            NPCDriver caughtCollector = GetCollectorInLungeRange();
            if (caughtCollector != null)
            {
				// Force collector to drop all souls
				CollectorStateHelper.DropSouls((caughtCollector as CollectorDriver), (caughtCollector as CollectorDriver).SoulsStored);

                NPCState caughtCollectorTransition = new CollectorImmortalState(caughtCollector.StateMachine);
                caughtCollectorTransition.Entry();
                caughtCollector.StateMachine.ChangeCurrentState(caughtCollectorTransition);
                lungeTimer = 0.0f;
                GameObject.FindGameObjectWithTag("GameController").SendMessage("loseLife");
            }
        }
        else
        {
            this.stateMachine.NPC.GetComponent<Rigidbody>().velocity = Vector3.zero;
            this.stateMachine.NPC.MovementDriver.enabled = true;
            (this.stateMachine as GuardStateMachine).LungeCooldown = GuardStateMachine.TIME_BETWEEN_LUNGES;
            return new GuardSearchState(this.stateMachine);
        }

        return this;
    }

    private NPCDriver GetCollectorInLungeRange()
    {
        foreach (NPCDriver npc in GameManager.Collectors)
        {
            if ((npc as CollectorDriver).StateMachine.CurrentState.GetType() == typeof(CollectorImmortalState)) continue;

            if (NPCStateHelper.IsWithinCollisionRangeAtGroundLevel(stateMachine.NPC.Instance, npc.Instance, GuardStateMachine.LUNGE_COLLISION_RANGE))
            {
                return npc;
            }
        }

        return null;
    }
}
