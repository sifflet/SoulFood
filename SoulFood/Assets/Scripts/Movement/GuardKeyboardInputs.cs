using UnityEngine;
using System.Collections;

public class GuardKeyboardInputs : KeyboardInputs
{
    public bool IsLeader { get; set; }

    protected override void HandleMovementInputs()
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
