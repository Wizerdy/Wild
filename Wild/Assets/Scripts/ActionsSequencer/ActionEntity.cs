using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionEntity : Action {
    [HideInInspector] public Entity entity;
    [HideInInspector] public Entity entityTriggering;
    [SerializeField] public TargetEntity targEntity;

    public override void Start() {
        base.Start();
        entity = targEntity.FindEntity();
    }

    public void Execute(Entity target) {
        entityTriggering = target;
        if (targEntity.targetType == TargetEntity.ENTITY_TARGET_TYPE.TARGET) {
            entity = entityTriggering;
        }

        base.Execute();
    }
}
