using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPause : ActionEntity {
    public float time;

    protected override void OnStart() { }

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
