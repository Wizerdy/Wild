using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionChangeAlpha : ActionEntity {
    public byte alpha;

    [HideInInspector] public byte currentAlpha;

    protected override void OnStart() {
        alpha = currentAlpha;
    }

    protected override void OnExecute() {
        Tools.ChangeAlphaMaterial(entity.gameObject, alpha);
    }

    public override bool IsActionEnded() {
        if (actionEnded) {
            return true;
        }
        return actionEnded;
    }
}
