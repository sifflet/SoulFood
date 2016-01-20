using UnityEngine;
using System.Collections;

public class SteeringManager
{
    private Vector3 steering;
    private GameObject instance;
    private Vector3 targetPosition;
    private UnitMovement movement;

    private const float MAX_SEE_AHEAD = 10f;
    private const float MAX_AVOID_FORCE = 25f;
    private const float MIN_VELOCITY_MAGNITUDE = 1f;

    public SteeringManager(GameObject instance)
    {
        this.instance = instance;
        this.steering = Vector3.zero;
        this.movement = instance.GetComponent<UnitMovement>();
        this.targetPosition = instance.transform.position;
    }

    public void SetTargetPosition(Vector3 target)
    {
        this.targetPosition = target;
        this.targetPosition.y = 0f;
    }

    public void Seek(Vector3 target, float stopDistance)
    {
        steering += ApplySeek(target, stopDistance);
    }

    public void Move()
    {
        steering += ApplyMove();
    }

    public void AvoidCollisions()
    {
        steering += ApplyCollisionAvoidance();
    }

    public void Update()
    {
        movement.velocity = steering;
        Cleanup();
    }

    private void Cleanup()
    {
        if (steering.magnitude < MIN_VELOCITY_MAGNITUDE)
        {
            steering = Vector3.zero;
            targetPosition = instance.transform.position;
        }
    }

    private Vector3 ApplySeek(Vector3 target, float stopDistance)
    {
        Vector3 result = Vector3.zero;
        Vector3 direction = (target - instance.transform.position).normalized;
        direction.y = 0f;
        float distance = Vector3.Distance(instance.transform.position, target);

        if (distance <= stopDistance)
        {
            result = direction * movement.speed * distance / stopDistance;
        }
        else
        {
            result = direction * movement.speed;
        }

        result = result - movement.velocity;
        return result;
    }

    private Vector3 ApplyMove()
    {
        Vector3 result = Vector3.zero;
        Vector3 direction = (targetPosition - instance.transform.position).normalized;
        float distance = Vector3.Distance(instance.transform.position, targetPosition);
        direction.y = 0f;

        if ((targetPosition - instance.transform.position).magnitude >= MIN_VELOCITY_MAGNITUDE)
        {
            if (distance <= movement.stopDistance)
            {
                result = direction * movement.speed * distance / movement.stopDistance;
            }
            else
            {
                result = direction * movement.speed;
            }
        }

        result = result - movement.velocity;
        return result;
    }

    private Vector3 ApplyCollisionAvoidance()
    {
        Vector3 result = Vector3.zero;
        Vector3 ahead = instance.transform.position + movement.velocity.normalized * MAX_SEE_AHEAD;
        Vector3 ahead2 = instance.transform.position + movement.velocity.normalized * MAX_SEE_AHEAD * 0.5f;

        //Vector3 ahead = instance.transform.forward * MAX_SEE_AHEAD;
        //Vector3 ahead2 = instance.transform.forward * MAX_SEE_AHEAD * 0.5f;

        GameObject closest = findClosestObstacle(ahead, ahead2);

        if (closest != null)
        {
            result = ahead - closest.transform.position;
            result = result.normalized;
            result *= MAX_AVOID_FORCE;
        }
        else
        {
            result = Vector3.zero;
        }

        return result;
    }

    private GameObject findClosestObstacle(Vector3 ahead, Vector3 ahead2)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Unit");
        GameObject closest = null;

        for (int i = 0; i < objects.Length; ++i)
        {
            bool collision = lineIntersectsCircle(ahead, ahead2, objects[i]);

            if (collision && (closest == null || Vector3.Distance(instance.transform.position, objects[i].transform.position) < Vector3.Distance(instance.transform.position, closest.transform.position)))
            {
                closest = objects[i];
            }
        }

        return closest;
    }

    private bool lineIntersectsCircle(Vector3 ahead, Vector3 ahead2, GameObject obj)
    {
        Renderer rend = obj.GetComponent<Renderer>();

        return Vector3.Distance(obj.transform.position, ahead) <= rend.bounds.extents.magnitude * 2
            || Vector3.Distance(obj.transform.position, ahead2) <= rend.bounds.extents.magnitude * 2;
    }
}
