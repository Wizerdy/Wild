using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionReset : ActionCamera {
    public CameraTransition transition;

    protected override void Start() {
        base.Start();
    }

    public override void Execute() {
        base.Execute();

        cam.RestoreDefaultProfile(transition);
    }

    public override bool IsActionEnded() {
        if (!cam.IsMoving) {
            actionEnded = true;
        }
        return actionEnded;
    }
}
