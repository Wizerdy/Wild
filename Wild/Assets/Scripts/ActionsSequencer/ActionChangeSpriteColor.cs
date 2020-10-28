using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionChangeSpriteColor : ActionEntity
{
    public Color spriteColor;

    [HideInInspector] public Color currentSpriteColor;

    protected override void Start()
    {
        base.Start();

        spriteColor = currentSpriteColor;
    }

    public override void Execute()
    {
        base.Execute();

        Tools.ChangeSpriteColor(entity.gameObject, spriteColor);
    }

    public override bool IsActionEnded()
    {
        if (actionEnded)
        {
            return true;
        }
        return actionEnded;
    }
}
