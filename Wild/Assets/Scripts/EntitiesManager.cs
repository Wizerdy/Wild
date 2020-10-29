using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EntitiesManager {
    public static List<Entity> entities = new List<Entity>();

    public static Entity FindEntity(string id) {
        for (int i = 0; i < entities.Count; i++)
            if (entities[i].entityId.Equals(id))
                return entities[i];

        Debug.LogError("Entity not found : " + id);
        return null;
    }

    public static Entity[] FindEntities(string group) {
        List<Entity> entitiesGroup = new List<Entity>();

        for (int i = 0; i < entities.Count; i++)
            if (entities[i].entityId.Equals(group))
                entitiesGroup.Add(entities[i]);

        if (entitiesGroup.Count <= 0)
            return null;
        else
            return entitiesGroup.ToArray();
    }

    public static void AddEntity(Entity entity) {
        entities.Add(entity);
    }
    
    public static void RemoveEntity(Entity entity) {
        entities.Remove(entity);
    }

    public static void ClearEntities() {
        entities.Clear();
    }

    public static void DebugEntities() {
        for (int i = 0; i < entities.Count; i++) {
            Debug.Log(entities[i].name);
        }
    }
}
