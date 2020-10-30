using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LionCubEntity : AnimalEntity {
    public string predatorGroup = "Hyenas";

    public Vector2 spawnPoint;

    //protected override void OnTriggerEnter(Collider collide) {
    //    base.OnTriggerEnter(collide);
    //    if (collide.gameObject.tag == "Hide") {
    //        if (collide.gameObject.GetComponent<Entity>() != null) {
    //            Entity[] entities = EntitiesManager.FindEntities(collide.gameObject.GetComponent<Entity>().entityGroup[1]);
    //            for (int i = 0; i < entities.Length; i++) {
    //                Tools.ChangeAlphaMaterial(entities[i].gameObject, 100);
    //            }
    //        } else {
    //            Tools.ChangeAlphaMaterial(collide.gameObject, 100);
    //        }
    //    }
    //}
    //protected override void OnTriggerExit(Collider collide) {
    //    base.OnTriggerExit(collide);
    //    if (hideCoat <= 0 && collide.gameObject.tag == "Hide") {
    //        if (collide.gameObject.GetComponent<Entity>() != null) {
    //            Entity[] entities = EntitiesManager.FindEntities(collide.gameObject.GetComponent<Entity>().entityGroup[1]);
    //            for (int i = 0; i < entities.Length; i++) {
    //                Tools.ChangeAlphaMaterial(entities[i].gameObject, 255);
    //            }
    //        } else {
    //            Tools.ChangeAlphaMaterial(collide.gameObject, 255);
    //        }
    //    }
    //}

    protected void OnCollisionEnter(Collision collision) {
        Entity entity = collision.gameObject.GetComponent<Entity>();
        if (entity == null) return;

        if(Array.IndexOf(entity.entityGroup, predatorGroup) >= 0) {
            Respawn();
        }
    }

    public void Respawn() {
        Entity[] hyenas = EntitiesManager.FindEntities("Hyenas");
        MoveInstant(spawnPoint.ConvertTo3D());

        if (hyenas == null) return;
        for (int i = 0; i < hyenas.Length; i++) {
            HyenaEntity hyena = hyenas[i].GetComponent<HyenaEntity>();
            if (hyena != null && hyena.HasPrey) {
                hyenas[i].GetComponent<HyenaEntity>().Patrol();
            }
        }
    }

    public void ChangeSpawnPoint(Vector3 position) {
        ChangeSpawnPoint(position.ConvertTo2D());
    }

    public void ChangeSpawnPoint(Vector2 position) {
        spawnPoint = position;
    }
}
