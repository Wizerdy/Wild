using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionChangeSpriteColor : ActionEntity {
    public Color spriteColor;

    [HideInInspector] public Color currentSpriteColor;

    protected override void OnStart() {
        spriteColor = currentSpriteColor;
    }

    protected override void OnExecute() {
        Tools.ChangeSpriteColor(entity.gameObject, spriteColor);
    }

    public override bool IsActionEnded() {
        if (actionEnded) {
            return true;
        }
        return actionEnded;
    }
}
