using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTriggerFX : ActionEntity
{
    public bool isActive;

    protected override void OnStart() { }

    protected override void OnExecute()
    {
        entity.gameObject.SetActive(isActive);
    }

    public override bool IsActionEnded()
    {
        return actionEnded;
    }
}
