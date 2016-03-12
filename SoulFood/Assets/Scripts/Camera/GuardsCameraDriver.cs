using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class GuardsCameraDriver : CameraDriver
{
    List<GameObject> targets;

    private const float yOffset = 5.0f;
    private const float zOffset = -5.0f;

    public GuardsCameraDriver(GameObject cameraInstance, List<GameObject> targets)
        : base(cameraInstance)
    {
        this.targets = targets;
    }

    public override void Update()
    {
        throw new NotImplementedException();
    }
}
