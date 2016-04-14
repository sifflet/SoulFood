using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectorKeyboardInputs : KeyboardInputs
{
    private KeyCode ejectSoulKey = KeyCode.LeftShift;
    private KeyCode consumeSoulKey = KeyCode.Space;

	private int hypotheticalMaximumSoulCapacity = 10;

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
			RecalculateSpeedBasedOnSoulConsumption();
        }
        
        if (Input.GetKeyDown(consumeSoulKey))
        {
            NPCActions.CmdConsumeSoul(this.npc as CollectorDriver);
			RecalculateSpeedBasedOnSoulConsumption();
        }
    }

	/**
	 * Method for speed adjustment based on number of consumed souls
	 */	
	private void RecalculateSpeedBasedOnSoulConsumption() 
	{
		CollectorDriver collectorDriver = this.gameObject.GetComponent<NPCDriver>() as CollectorDriver;
		float speedDeboost = 0; // Variable is brought out of the if for printing purposes
		CollectorKeyboardInputs.maximumSpeed = CollectorKeyboardInputs.MAX_SPEED;	// Reset to max speed
		float startSpeed = CollectorKeyboardInputs.maximumSpeed; // For printing purposes
		if (collectorDriver) {
			float speedDecreaseFactor = (CollectorKeyboardInputs.maximumSpeed - CollectorKeyboardInputs.minimumSpeed) / this.hypotheticalMaximumSoulCapacity;
			speedDeboost = speedDecreaseFactor * collectorDriver.SoulsStored;
			if (CollectorKeyboardInputs.maximumSpeed - speedDeboost >= CollectorKeyboardInputs.minimumSpeed)
				CollectorKeyboardInputs.maximumSpeed -= speedDeboost;
			else
				CollectorKeyboardInputs.maximumSpeed = CollectorKeyboardInputs.minimumSpeed;
		}

		Debug.Log (collectorDriver.name + ": Souls -> " + collectorDriver.SoulsStored 
		           + ". Deboost -> " + speedDeboost
		           + ". Start Speed -> " + startSpeed
		           + ". Current Speed -> " + CollectorKeyboardInputs.maximumSpeed);

	}
}