using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class UnitManager
{
    public Transform spawnPoint;
    [HideInInspector] public GameObject instance;

    private Unit unit;
    private SteeringManager steering;

    public UnitManager()
    {
    }

    public void Setup()
    {
        unit = instance.GetComponent<Unit>();
        steering = new SteeringManager(instance);
    }

    public bool IsSelected()
    {
        return unit.selected;
    }

    public void Update()
    {
        steering.Move();
        //steering.AvoidCollisions();
        steering.Update();
    }

    public void Move(Vector3 destination)
    {
        steering.SetTargetPosition(destination);
    }
}
