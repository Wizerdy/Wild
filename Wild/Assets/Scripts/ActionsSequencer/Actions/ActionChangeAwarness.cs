using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionChangeAwarness : ActionEntity {
    public HyenaEntity.Awarness awarness;
    public GameObject prey = null;

    protected override void OnStart() { }

    protected override void OnExecute() {
        if (entity.GetComponent<HyenaEntity>() == null) { Debug.LogWarning("Not a hyena"); return; }

        entity.GetComponent<HyenaEntity>().ChangeState(awarness, prey);
    }

    public override bool IsActionEnded() {
        if (actionEnded) {
            return true;
        }
        return actionEnded;
    }
}
