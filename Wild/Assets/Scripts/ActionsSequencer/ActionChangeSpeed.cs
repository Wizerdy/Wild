using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionChangeSpeed : ActionEntity
{
    public int speed;

    [HideInInspector] public int currentSpeed;

    protected override void Start()
    {
        base.Start();

        speed = currentSpeed;
    }

    public override void Execute()
    {
        base.Execute();

        Tools.ChangeSpeed(entity.gameObject, speed);
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
