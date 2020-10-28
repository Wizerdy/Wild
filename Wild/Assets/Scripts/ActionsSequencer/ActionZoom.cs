using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionZoom : ActionCamera
{
    public Vector3 destination = new Vector3();
    public float time = 0f;
    public float percentage;
    public int steps = 0;

    [HideInInspector] public int destinationCurrentTab;
    [HideInInspector] public Transform goDestination;
    [HideInInspector] public Vector3 vectorDestination;

    protected override void Start()
    {
        base.Start();

        switch (destinationCurrentTab)
        {
            case 0:
                destination = goDestination.position;
                break;
            case 1:
                destination = vectorDestination;
                break;
        }
    }

    public override void Execute()
    {
        base.Execute();

        cam.Zoom(destination, time, steps, percentage);
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
