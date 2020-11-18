using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionChangeAnim : ActionEntity {
    public Animation anim;

    protected override void OnStart() { }

    protected override void OnExecute() {
        Tools.ChangeAnim(entity.gameObject, anim);
    }

    public override bool IsActionEnded() {
        if (actionEnded) {
            return true;
        }
        return actionEnded;
    }
}
