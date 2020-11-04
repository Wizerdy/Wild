using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSpawnPoint : ActionEntity
{
    public Vector2 position;

    protected override void Start()
    {
        base.Start();
    }

    public override void Execute()
    {
        base.Execute();

        entity.GetComponent<LionCubEntity>().ChangeSpawnPoint(position);
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
