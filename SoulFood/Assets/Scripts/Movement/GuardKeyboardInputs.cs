using UnityEngine;
using System.Collections;

public class GuardKeyboardInputs : KeyboardInputs
{
    public bool IsLeader { get; set; }

    private KeyCode leaderLungeKey = KeyCode.Space;
    private KeyCode followerLungeKey = KeyCode.Keypad0;

    private float lungeCooldown = 0f;

    protected override void HandleMovementInputs()
    {
        if (!IsLungeing())
        {
            if (IsLeader)
            {
                movement.x = Input.GetAxis("Horizontal2");
                movement.z = Input.GetAxis("Vertical2");
            }
            else
            {
                movement.x = Input.GetAxis("Horizontal");
                movement.z = Input.GetAxis("Vertical");
            }
        }
    }

    protected override void HandleActionInputs()
    {
        if (!IsLungeing() && lungeCooldown <= 0)
        {
            if (IsLeader)
            {
                if(Input.GetKeyDown(leaderLungeKey)) StartLunge();
            }
            else
            {
                if (Input.GetKeyDown(followerLungeKey)) StartLunge();
            }
        }

        if (lungeCooldown > 0)
        {
            lungeCooldown -= Time.deltaTime;
        }
        else
        {
            lungeCooldown = 0;
        }
    }

    private void StartLunge()
    {
        this.lungeCooldown = GuardStateMachine.TIME_BETWEEN_LUNGES;
        this.npc.MovementDriver.enabled = true;
        this.npc.StateMachine.enabled = true;

        NPCState transition = new GuardLungeState(npc.StateMachine);
        this.npc.StateMachine.ChangeCurrentState(transition);
        transition.Entry();
    }

    private void StopLunge()
    {
        this.npc.MovementDriver.enabled = false;
        this.npc.StateMachine.enabled = false;
    }

    private bool IsLungeing()
    {
        if (!npc.StateMachine.enabled) return false;
        if (npc.StateMachine.CurrentState.GetType() != typeof(GuardLungeState))
        {
            StopLunge();
            return false;
        }

        return true;
    }
}
