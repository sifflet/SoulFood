﻿using UnityEngine;
using System.Collections;

public class KeyboardInputs : MonoBehaviour
{
    public static float maximumSpeed = 5f;
    public static float minimumSpeed = 3.0f;

    protected Vector3 movement;
    protected float rotationSpeed = 3.0f;
    protected float speed = maximumSpeed;

	void Start ()
    {
        movement = Vector3.zero;
	}

	void Update ()
    {
        HandleInputs();
	}

    void FixedUpdate()
    {
        Move();
        Turn();
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