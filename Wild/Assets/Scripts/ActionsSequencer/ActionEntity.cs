using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionEntity : Action {
    [HideInInspector] public Entity entity;

    //Editor
    [HideInInspector] public Entity entityGo;
    [HideInInspector] public string entityId;
    [HideInInspector] public int entityCurrentTab;

    protected override void Start() {
        base.Start();

        switch(entityCurrentTab) {
            case 0:
                entity = EntitiesManager.FindEntity(entityId);
                break;
            case 1:
                entity = entityGo;
                break;
        }
    }
}
