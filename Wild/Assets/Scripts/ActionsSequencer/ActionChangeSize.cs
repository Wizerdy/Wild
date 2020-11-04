using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionChangeSize : ActionEntity
{
    public int size;

    [HideInInspector] public int currentSize;

    protected override void Start()
    {
        base.Start();

        size = currentSize;
    }

    public override void Execute()
    {
        base.Execute();

        Tools.ChangeSize(entity.gameObject, size);
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
