using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionChangeSize : ActionEntity {
    public int size;

    [HideInInspector] public int currentSize;

    protected override void OnStart() {
        size = currentSize;
    }

    protected override void OnExecute() {

        Tools.ChangeSize(entity.gameObject, size);
    }

    public override bool IsActionEnded() {
        if (actionEnded) {
            return true;
        }
        return actionEnded;
    }
}
