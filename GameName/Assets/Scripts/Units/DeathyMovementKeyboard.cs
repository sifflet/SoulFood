using UnityEngine;
using System.Collections;

public class DeathyMovementKeyboard : MonoBehaviour
{
    public float speed = 1f;

    private Vector3 movement;
    private Animator animator;

	void Start ()
    {
        animator = GetComponent<Animator>();
	}

	void Update ()
    {
        GetInputs();
	}

    void FixedUpdate()
    {
        Move();
        Turn();
    }

    private void GetInputs()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.z = Input.GetAxis("Vertical");
    }

    private void Move()
    {
        GetComponent<Rigidbody>().velocity = movement * speed;

        if (movement.sqrMagnitude > 0f)
        {
            animator.SetFloat("speed", 1);
        }
        else
        {
            animator.SetFloat("speed", 0);
        }
    }

    private void Turn()
    {
        Vector3 direction = movement.normalized;
        float step = speed * Time.deltaTime;

        Vector3 rotation = Vector3.RotateTowards(transform.forward, direction, step, 0.0f);
        Debug.DrawRay(transform.position, rotation, Color.red);
        transform.rotation = Quaternion.LookRotation(rotation);
    }
}
