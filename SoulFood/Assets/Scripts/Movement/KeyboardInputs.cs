using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class KeyboardInputs : NetworkBehaviour
{
	public const float MAX_SPEED = 5f;
	public static float maximumSpeed = MAX_SPEED;
	public static float minimumSpeed = 3.0f;

    protected Vector3 movement;
    protected float rotationSpeed = 3.0f;
    public float speed = maximumSpeed;

    protected NPCDriver npc;

	void Start ()
    {
        movement = Vector3.zero;
	}

	void Update ()
    {
        /*
        if (!isLocalPlayer)
        {
            return;
        }
        */
        HandleInputs();
	}

    void FixedUpdate()
    {
        Move();
        Turn();
    }

    public void Setup(NPCDriver npc)
    {
        this.npc = npc;
    }

    protected virtual void HandleInputs()
    {
        HandleMovementInputs();
        HandleActionInputs();
    }

    protected virtual void HandleMovementInputs() { }

    protected virtual void HandleActionInputs() { }

    protected void Move()
    {
        GetComponent<Rigidbody>().velocity = movement * speed;
    }

    protected void Turn()
    {
        Vector3 direction = movement.normalized;
        float step = rotationSpeed * Time.deltaTime;

        Vector3 rotation = Vector3.RotateTowards(transform.forward, direction, step, 0.0f);
        Debug.DrawRay(transform.position, rotation, Color.red);
        transform.rotation = Quaternion.LookRotation(rotation);
    }
}
