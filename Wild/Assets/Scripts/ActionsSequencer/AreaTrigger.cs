using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
[ExecuteInEditMode]
public class AreaTrigger : MonoBehaviour {
    public string entityGroup = "";
    private ActionSequencer enterActions;
    private ActionSequencer exitActions;

    public bool alwaysExitActions = false;

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

        if (gameObject.name.Equals("GameObject")) { gameObject.name = "AreaTrigger"; }
    }

    public void OnAreaEnter() {
        enterActions.Launch();
    }

    public void OnAreaExit() {
        exitActions.Launch();
    }

    private void OnTriggerEnter(Collider other) {
        Entity otherEntity = other.GetComponentInParent<Entity>();
        if (null == otherEntity) return;
        if (!string.IsNullOrWhiteSpace(entityGroup) && Array.IndexOf(otherEntity.entityGroup, entityGroup) < 0) return;

        otherEntity.EnterAreaTrigger(this);
    }

    private void OnTriggerExit(Collider other) {
        Entity otherEntity = other.GetComponentInParent<Entity>();
        if (null == otherEntity) return;
        if (!string.IsNullOrWhiteSpace(entityGroup) && Array.IndexOf(otherEntity.entityGroup, entityGroup) < 0) return;

        otherEntity.ExitAreaTrigger(this);
    }

    private void OnDrawGizmos() {
        Color color = gizmosColor;
        color.a = 0.5f;
        Gizmos.color = color;
        Gizmos.DrawCube(transform.position + boxCollider.center, boxCollider.size);
    }
}