using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
[ExecuteInEditMode]
public class CollisionActions : MonoBehaviour
{
    public string entityGroup = "";
    private ActionSequencer enterActions;
    private ActionSequencer exitActions;

    public Color gizmosColor = Color.red;

    private Rigidbody rigidBody;
    private BoxCollider boxCollider;

    private void Awake() {
        rigidBody = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        rigidBody.isKinematic = true;
        rigidBody.useGravity = false;
        boxCollider.isTrigger = true;

        foreach (ActionSequencer sequencer in GetComponentsInChildren<ActionSequencer>()) {
            if (sequencer.name == "OnEnter") {
                enterActions = sequencer;
            } else if (sequencer.name == "OnExit") {
                exitActions = sequencer;
            }
        }

        if (null == enterActions) {
            GameObject enterActionGo = new GameObject("OnEnter");
            enterActionGo.transform.SetParent(transform);
            enterActions = enterActionGo.AddComponent<ActionSequencer>();
        }

        if (null == exitActions) {
            GameObject exitActionGo = new GameObject("OnExit");
            exitActionGo.transform.SetParent(transform);
            exitActions = exitActionGo.AddComponent<ActionSequencer>();
        }

        if (String.IsNullOrWhiteSpace(gameObject.name)) { gameObject.name = "AreaTrigger"; }
    }

    private void OnTriggerEnter(Collider other) {
        Entity otherEntity = other.GetComponentInParent<Entity>();
        if (null == otherEntity) return;
        if (!string.IsNullOrWhiteSpace(entityGroup) && Array.IndexOf(otherEntity.entityGroup, entityGroup) < 0) return;

        enterActions.Launch();
    }

    private void OnTriggerExit(Collider other) {
        Entity otherEntity = other.GetComponentInParent<Entity>();
        if (null == otherEntity) return;
        if (!string.IsNullOrWhiteSpace(entityGroup) && Array.IndexOf(otherEntity.entityGroup, entityGroup) < 0) return;

        exitActions.Launch();
    }

    private void OnDrawGizmos() {
        if(boxCollider == null) { return; }

        Color color = gizmosColor;
        color.a = 0.75f;
        Gizmos.color = color;
        Gizmos.DrawCube(transform.position + boxCollider.center, boxCollider.size);
    }
}
