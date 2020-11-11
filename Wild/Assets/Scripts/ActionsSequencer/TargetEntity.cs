using System;

[Serializable]
public class TargetEntity {
    public enum ENTITY_TARGET_TYPE {
        ENTITY_ID = 0,
        ENTITY,
    }

    public ENTITY_TARGET_TYPE targetType = ENTITY_TARGET_TYPE.ENTITY;

    public Entity entityGo;
    public string entityId;

    public Entity FindEntity() {
        switch (targetType) {
            case ENTITY_TARGET_TYPE.ENTITY_ID:
                return EntitiesManager.FindEntity(entityId);

            case ENTITY_TARGET_TYPE.ENTITY:
                return entityGo;
        }

        return null;
    }

    public string EntityName() {
        string name = "";

        if (targetType == ENTITY_TARGET_TYPE.ENTITY_ID && String.Compare(entityId, "") != 0) { name = entityId; } else if (targetType == ENTITY_TARGET_TYPE.ENTITY && entityGo != null) { name = entityGo.name; }

        return name;
    }
}
