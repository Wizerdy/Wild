using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMoveCamera : ActionCamera {
    public Vector3 destination = new Vector3();
    public CameraTransition transition;
    public Camera camTemp;

    protected override void OnStart() { }

    protected override void OnExecute() {
        cam.ApplyCameraProfile(camTemp, CameraManager.PROFILE_MODE.OVERRIDE, transition);
    }

    public override bool IsActionEnded() {
        if (!cam.IsMoving) {
            actionEnded = true;
        }
        return actionEnded;
    }
}
