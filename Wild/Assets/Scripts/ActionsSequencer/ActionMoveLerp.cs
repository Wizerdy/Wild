using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMoveLerp : ActionEntity {
    public Vector3 destination = new Vector3();
    public float time = 0f;
    public int steps = 0;

    // Editor
    [HideInInspector] public int destinationCurrentTab;
    [HideInInspector] public Transform goDestination;
    [HideInInspector] public Vector3 vectorDestination;

    protected override void Start() {
        base.Start();

        switch (destinationCurrentTab) {
            case 0:
                destination = goDestination.position;
                break;
            case 1:
                destination = vectorDestination;
                break;
        }
    }

    public override void Execute() {
        base.Execute();

        entity.DoMoveLerp(destination, time, steps);
    }

    public override bool IsActionEnded() {
        if (!entity.IsMovementForced) {
            actionEnded = true;
        }
        return actionEnded;
    }
}
