using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionEntity : Action {
    [HideInInspector] public Entity entity;
    [SerializeField] public TargetEntity targEntity;

    protected override void OnStart() {
        entity = targEntity.FindEntity();
    }
}
