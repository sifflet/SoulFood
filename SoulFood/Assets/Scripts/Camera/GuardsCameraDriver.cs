using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class GuardsCameraDriver : CameraDriver
{
    private const float yOffset = 5.0f;
    private const float zOffset = -5.0f;

    public GuardsCameraDriver(GameObject cameraInstance)
        : base(cameraInstance)
    {
    }

    public override void Update()
    {
    }
}
