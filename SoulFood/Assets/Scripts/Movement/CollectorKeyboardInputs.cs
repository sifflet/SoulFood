using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectorKeyboardInputs : KeyboardInputs
{
    public static int maximumCandyCapacity = 20;
    private Stack<GameObject> candyContainer = new Stack<GameObject>(maximumCandyCapacity);
    private GameObject candyCollidedWith;

    private KeyCode pickupCandyKey = KeyCode.LeftControl;

    // Degrees per second
    float angularSpeed = 180.0f;

    protected override void HandleMovementInputs()
    {
        movement.x = Input.GetAxis("Horizontal2");
        movement.z = Input.GetAxis("Vertical2");
    }

    /*  
     *  Handle button input events
     *  @return: void
     */
    protected override void HandleActionInputs()
    {
        //TODO: holding button loses candies at fixed rate
        if (Input.GetButtonDown("DropCandy") && this.candyContainer.Count > 0)
        {
            this.DropCandy();
        }
        
        if (Input.GetKeyDown(pickupCandyKey))
        {
            NPCActions.ConsumeSoul(this.npc as CollectorDriver);
        }
    }

    /*  
     *  Determine player speed based on amount of candy currently held
     *  @return: void
     */
    private void CalculateSpeed()
    {
        this.speed = maximumSpeed - (((maximumSpeed - minimumSpeed) / maximumCandyCapacity) * this.candyContainer.Count);
    }

    /*  
     *  Player picks up candy, updates player speed, removes candy from playing field
     *  @param: candy being collided with
     *  @return: void
     */
    private void PickUpCandy(GameObject candy)
    {
        this.candyContainer.Push(candy);
        candy.SetActive(false);
        this.CalculateSpeed();
    }

    /*  
     *  Player drops candy, updates player speed, returns candy to playing field
     *  @return: void
     */
    private void DropCandy()
    {
        GameObject droppedCandy = this.candyContainer.Pop();
        droppedCandy.SetActive(true);
        droppedCandy.transform.position = this.transform.position - this.transform.forward;

        this.CalculateSpeed();
    }

    /*  
     *  Player drops all candy, updates player speed, returns all candy to playing field
     *  @return: void
     */
    private void DropAllCandy()
    {
        foreach (GameObject candy in this.candyContainer)
        {
            candy.SetActive(true);
            candy.transform.position = this.transform.position - this.transform.forward;
        }
        this.candyContainer.Clear();

        this.CalculateSpeed();
    }

    /*  
     *  Handle player being hit by enemy
     *  @return: void
     */
    private void HitByEnemy()
    {
        if (this.candyContainer.Count > 0)
        {
            this.DropAllCandy();
            // TODO:
            // reduce team lives
            // check who wins game: enemies or players
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Guard")
        {
            print("Player hit!");
            this.HitByEnemy();
        }

        if (collision.gameObject.tag == "Soul")
        {
            this.candyCollidedWith = collision.gameObject;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Soul")
        {
            this.candyCollidedWith = null;
        }
    }
}
