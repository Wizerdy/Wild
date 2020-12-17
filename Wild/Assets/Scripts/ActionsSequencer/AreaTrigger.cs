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
    private BoxCollider[] boxCollider;

    private void Awake() {
        rigidBody = GetComponent<Rigidbody>();
        boxCollider = GetComponents<BoxCollider>();
        rigidBody.isKinematic = true;
        rigidBody.useGravity = false;
        for (int i = 0; i < boxCollider.Length; i++) {
            boxCollider[i].isTrigger = true;
        }

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

    public void OnAreaEnter(Entity entity = null) {
        enterActions.Launch(entity);
    }

    public void OnAreaExit(Entity entity = null) {
        exitActions.Launch(entity);
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
        Gizmos.color = color;
        for (int i = 0; i < boxCollider.Length; i++) {
            Gizmos.DrawCube(transform.position + boxCollider[i].center, boxCollider[i].size);
        }
    }
}