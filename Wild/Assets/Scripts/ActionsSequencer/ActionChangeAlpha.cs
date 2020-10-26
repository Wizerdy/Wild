using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionChangeAlpha : ActionEntity
{
    public byte alpha;

    [HideInInspector] public byte currentAlpha;

    protected override void Start()
    {
        base.Start();

        alpha = currentAlpha;
    }

    public override void Execute()
    {
        base.Execute();

        Tools.ChangeAlphaMaterial(entity.gameObject, alpha);
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
