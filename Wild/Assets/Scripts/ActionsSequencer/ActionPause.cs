using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPause : ActionEntity
{
    public float time;

    [HideInInspector] public float currentTime;

    protected override void Start()
    {
        base.Start();

        time = currentTime;
    }

    public override void Execute()
    {
        base.Execute();

        entity.DoMoveLerp(entity.transform.position, time, 1);
    }

    public override bool IsActionEnded()
    {
        if (!entity.IsMovementForced)
        {
            actionEnded = true;
        }
        return actionEnded;
    }
}
