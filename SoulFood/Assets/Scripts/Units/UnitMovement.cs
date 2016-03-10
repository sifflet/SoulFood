using UnityEngine;
using System.Collections;

public class UnitMovement : MonoBehaviour
{
    public float speed = 12f;
    public float turnSpeed = 2f;
    public float floorOffset = 1f;
    public float stopDistance = 1.0f;
    [HideInInspector] public Vector3 velocity = Vector3.zero;

    private Rigidbody rigidBody;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

	private void Start ()
    {
	
	}
	
	private void Update ()
    {
	}

    private void FixedUpdate()
    {
        Move();
        Turn();
    }

    private void Move()
    {
        rigidBody.velocity = velocity;
    }

    private void Turn()
    {
        /*
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0;
        float step = turnSpeed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, direction, step, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
         * */
    }
}
