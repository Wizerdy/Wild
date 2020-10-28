using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionReset : ActionCamera
{
    public float time = 0f;
    public int steps = 0;

    protected override void Start()
    {
        base.Start();
    }

    public override void Execute()
    {
        base.Execute();

        cam.ResetCameraToBase(time, steps);
    }

    public override bool IsActionEnded()
    {
        if (!cam.cameraEntity.IsMovementForced)
        {
            actionEnded = true;
        }
        return actionEnded;
    }
}
