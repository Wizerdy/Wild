using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionChangeAnim : ActionEntity
{
    public Animation anim;

    protected override void Start()
    {
        base.Start();
    }

    public override void Execute()
    {
        base.Execute();

        Tools.ChangeAnim(entity.gameObject, anim);
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
