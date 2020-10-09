using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LionCubEntity : AnimalEntity {
    protected override void OnTriggerEnter(Collider collide) {
        base.OnTriggerEnter(collide);
        if (collide.gameObject.tag == "Hide") {
            ChangeAlphaMaterial(collide.gameObject, 100);
        }
    }

    protected override void OnTriggerExit(Collider collide) {
        base.OnTriggerExit(collide);
        if (collide.gameObject.tag == "Hide") {
            ChangeAlphaMaterial(collide.gameObject, 255);
        }
    }
}