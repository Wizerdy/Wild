using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPause : ActionEntity {
    public float time;

    [HideInInspector] public float currentTime;

    protected override void OnStart() {
        time = currentTime;
    }

    protected override void OnExecute() {
        entity.DoMoveLerp(entity.transform.position, time);
    }

    public override bool IsActionEnded() {
        if (!entity.IsMovementForced) {
            actionEnded = true;
        }
        return actionEnded;
    }
}
