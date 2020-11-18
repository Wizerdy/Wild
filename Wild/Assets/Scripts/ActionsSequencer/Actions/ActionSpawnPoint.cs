using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSpawnPoint : ActionEntity {
    public Vector3 position;

    protected override void OnStart() { }

    protected override void OnExecute() {
        entity.GetComponent<LionCubEntity>().ChangeSpawnPoint(position);
    }

    public override bool IsActionEnded() {
        if (actionEnded) {
            return true;
        }
        return actionEnded;
    }
}
