using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectorKeyboardInputs : KeyboardInputs
{
    private KeyCode ejectSoulKey = KeyCode.LeftShift;
    private KeyCode consumeSoulKey = KeyCode.Space;

    protected override void HandleMovementInputs()
    {
        movement.x = Input.GetAxis("Horizontal2");
        movement.z = Input.GetAxis("Vertical2");
    }

    protected override void HandleActionInputs()
    {
        if (Input.GetKeyDown(ejectSoulKey))
        {
            NPCActions.CmdEjectSoul(this.npc as CollectorDriver, 1);
        }
        
        if (Input.GetKeyDown(consumeSoulKey))
        {
            NPCActions.CmdConsumeSoul(this.npc as CollectorDriver);
        }
    }   
}