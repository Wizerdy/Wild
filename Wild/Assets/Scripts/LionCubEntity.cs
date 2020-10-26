using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LionCubEntity : AnimalEntity {
    protected override void OnTriggerEnter(Collider collide) {
        base.OnTriggerEnter(collide);
        if (collide.gameObject.tag == "Hide") {
            if (collide.gameObject.GetComponent<Entity>() != null) {
                Entity[] entities = EntitiesManager.FindEntities(collide.gameObject.GetComponent<Entity>().entityGroup[1]);
                for (int i = 0; i < entities.Length; i++) {
                    ChangeAlphaMaterial(entities[i].gameObject, 100);
                }
            } else {
                ChangeAlphaMaterial(collide.gameObject, 100);
            }
        }
    }

    protected override void OnTriggerExit(Collider collide) {
        base.OnTriggerExit(collide);
        if (hideCoat <= 0 && collide.gameObject.tag == "Hide") {
            if (collide.gameObject.GetComponent<Entity>() != null) {
                Entity[] entities = EntitiesManager.FindEntities(collide.gameObject.GetComponent<Entity>().entityGroup[1]);
                for (int i = 0; i < entities.Length; i++) {
                    ChangeAlphaMaterial(entities[i].gameObject, 255);
                }
            } else {
                ChangeAlphaMaterial(collide.gameObject, 255);
            }
        }
    }
}