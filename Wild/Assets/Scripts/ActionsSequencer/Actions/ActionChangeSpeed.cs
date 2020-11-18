using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionChangeSpeed : ActionEntity {
    public int speed;

    [HideInInspector] public int currentSpeed;

    protected override void OnStart() {
        speed = currentSpeed;
    }

    protected override void OnExecute() {
        Tools.ChangeSpeed(entity.gameObject, speed);
    }

    public override bool IsActionEnded() {
        if (actionEnded) {
            return true;
        }
        return actionEnded;
    }

}
