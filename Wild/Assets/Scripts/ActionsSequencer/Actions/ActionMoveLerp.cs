using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMoveLerp : ActionEntity {
    public Vector3 destination = new Vector3();
    public float time = 0f;

    public TargetDestination targDestination;
    // Editor
    //[HideInInspector] public int destinationCurrentTab;
    //[HideInInspector] public Transform goDestination;
    //[HideInInspector] public Vector3 vectorDestination;

    protected override void OnStart() { }

    protected override void OnExecute() {
        destination = targDestination.FindDestination();
        entity.DoMoveLerp(destination, time);
    }

    public override bool IsActionEnded() {
        if (!entity.IsMovementForced) {
            actionEnded = true;
        }
        return actionEnded;
    }
}
