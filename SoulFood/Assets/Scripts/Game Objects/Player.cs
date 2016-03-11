using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    private Vector3 movement;
    public static float maximumSpeed = 5f;
    public static float minimumSpeed = 3.0f;
    private float rotationSpeed = 3.0f;
    private float speed = maximumSpeed;

    public static int maximumCandyCapacity = 20;
    private Stack<GameObject> candyContainer = new Stack<GameObject>(maximumCandyCapacity);
    private GameObject candyCollidedWith;

    private Animator animator;

	// Degrees per second
	float angularSpeed = 180.0f;

    void Start ()
    {
        movement = Vector3.zero;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleInputs();
    }

    void FixedUpdate()
    {
        Move();
        Turn();
        movement = Vector3.zero;
    }

    private void HandleInputs()
    {
        HandleMovementInputs();
        HandleActionInputs();
    }

    private void HandleMovementInputs()
    {
        movement = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) movement += transform.forward;
        if (Input.GetKey(KeyCode.A)) movement += -transform.right;
        if (Input.GetKey(KeyCode.S)) movement += -transform.forward;
        if (Input.GetKey(KeyCode.D)) movement += transform.right;

        movement = movement.normalized;
    }

    /*  
    *  Handle movement and rotation towards movement direction
    *  @return: void
    */
    private void Move()
    {
        GetComponent<Rigidbody>().velocity = movement * speed;
        /*
        if (movement.sqrMagnitude > 0f)
        {
            animator.SetFloat("speed", 1);
        }
        else
        {
            animator.SetFloat("speed", 0);
        }
         * */
    }

    private void Turn()
    {
        Vector3 direction = movement.normalized;
        float step = rotationSpeed * Time.deltaTime;

        Vector3 rotation = Vector3.RotateTowards(transform.forward, direction, step, 0.0f);
        Debug.DrawRay(transform.position, rotation, Color.red);
        transform.rotation = Quaternion.LookRotation(rotation);
    }

    /*  
     *  Handle button input events
     *  @return: void
     */
    private void HandleActionInputs()
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