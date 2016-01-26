using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public static float maximumSpeed = 10.0f;
    public static float minimumSpeed = 3.0f;
    private float speed = maximumSpeed;

    public static int maximumCandyCapacity = 20;
    private Stack<GameObject> candyContainer = new Stack<GameObject>(maximumCandyCapacity);
    private GameObject candyCollidedWith;

    void Update()
    {
        this.HandleButtonInput();
        this.Move();
    }

    /*  
     *  Handle movement
     *  @return: void
     */
    private void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = Vector3.ClampMagnitude(new Vector3(horizontal, 0.0f, vertical), 1.0f);

        this.transform.Translate(direction * this.speed * Time.deltaTime);
    }

    /*  
     *  Handle button input events
     *  @return: void
     */
    private void HandleButtonInput()
    {
        //TODO: holding button loses candies at fixed rate
        if (Input.GetButtonDown("DropCandy") && this.candyContainer.Count > 0)
        {
            this.DropCandy();
        }

        if(Input.GetButtonDown("PickUpCandy") && this.candyCollidedWith != null && this.candyContainer.Count < maximumCandyCapacity)
        {
            this.PickUpCandy(this.candyCollidedWith);
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
        if (collision.gameObject.tag == "Enemy")
        {
            print("Player hit!");
            this.HitByEnemy();
        }

        if (collision.gameObject.tag == "Candy")
        {
            this.candyCollidedWith = collision.gameObject;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Candy")
        {
            this.candyCollidedWith = null;
        }
    }
}