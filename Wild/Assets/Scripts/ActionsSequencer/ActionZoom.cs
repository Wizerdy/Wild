using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionZoom : ActionCamera {
    public Vector3 destination = new Vector3();
    public float percentage;
    public CameraTransition transition;

    [HideInInspector] public int destinationCurrentTab;
    [HideInInspector] public Transform goDestination;
    [HideInInspector] public Vector3 vectorDestination;

    protected override void Start() {
        base.Start();
    }

    public override void Execute() {
        base.Execute();

        switch (destinationCurrentTab) {
            case 0:
                destination = goDestination.position;
                break;
            case 1:
                destination = vectorDestination;
                break;
        }

        cam.Zoom(destination, percentage, transition);
    }

    public override bool IsActionEnded() {
        if (!cam.IsMoving) {
            actionEnded = true;
        }
        return actionEnded;
    }
}
