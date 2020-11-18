using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionZoom : ActionCamera {
    [HideInInspector] public Vector3 destination = Vector3.zero;
    public float percentage;
    public CameraTransition transition;

    [SerializeField] public TargetDestination targDestination;

    protected override void OnStart() { }

    protected override void OnExecute() {
        destination = targDestination.FindDestination();

        cam.Zoom(destination, percentage, transition);
    }

    public override bool IsActionEnded() {
        if (!cam.IsMoving) {
            actionEnded = true;
        }
        return actionEnded;
    }
}
