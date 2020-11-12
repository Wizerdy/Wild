using System;

[Serializable]
public class TargetEntity {
    public enum ENTITY_TARGET_TYPE {
        ENTITY_ID = 0,
        ENTITY,
        TARGET
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

        switch (targetType) {
            case ENTITY_TARGET_TYPE.ENTITY_ID:
                if (!String.IsNullOrWhiteSpace(entityId)) {
                    name = entityId;
                }
                break;
            case ENTITY_TARGET_TYPE.ENTITY:
                if (entityGo != null) {
                    name = entityGo.name;
                }
                break;
            case ENTITY_TARGET_TYPE.TARGET:
                name = "TARGET";
                break;
        }

        return name;
    }
}
