using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EntitiesManager {
    public static List<Entity> entities = new List<Entity>();

    public static Entity FindEntity(string id) {
        for (int i = 0; i < entities.Count; i++)
            if (String.Compare(entities[i].entityId, id) == 0)
                return entities[i];

        return null;
    }

    public static Entity[] FindEntities(string group) {
        List<Entity> entitiesGroup = new List<Entity>();

        for (int i = 0; i < entities.Count; i++)
            if (String.Compare(entities[i].entityId, group) == 0)
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
