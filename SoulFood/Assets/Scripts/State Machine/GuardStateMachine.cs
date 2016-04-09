using UnityEngine;
using System.Collections;

public class GuardStateMachine : NPCStateMachine
{
    public NPCDriver TargetNPC { get; set; }

    public const float ACTIVATE_LUNGE_DISTANCE = 4.0f;
    public const float DIRECT_PURSUE_RANGE = 5.0f;
    public const float LUNGE_TIME = 0.25f;
    public static float LUNGE_CONE_ANGLE = 30.0f;
    public const float LUNGE_COLLISION_RANGE = 1f;
    public const float PURSUE_NEW_TARGET_TIME = 5.0f;
    public const float CHANGE_TARGET_RANGE = 8.0f;
    public const float TIME_BETWEEN_LUNGES = 3.0f;

    public float LungeCooldown { get; set; }

    public override void Setup(NPCDriver npc)
    {
        base.Setup(npc);
        this.TargetNPC = null;
        this.currentState = new GuardSearchState(this);
    }

    public override void Reset()
    {
        this.currentState = new GuardSearchState(this);
    }

    public void ChangeCurrentState(NPCState newState)
    {
        this.currentState = newState;
    }
}
