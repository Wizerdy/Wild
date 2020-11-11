using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionReset : ActionCamera {
    public CameraTransition transition;

    protected override void OnStart() {

    }

    protected override void OnExecute() {
        cam.RestoreDefaultProfile(transition);
    }

    public override bool IsActionEnded() {
        if (!cam.IsMoving) {
            actionEnded = true;
        }
        return actionEnded;
    }
}
