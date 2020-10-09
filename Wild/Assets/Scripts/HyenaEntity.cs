using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HyenaEntity : AnimalEntity
{
    public enum Awarness {
        PATROLLING, CHASING
    }

    [Header("Patrolling")]
    public Vector2[] patrolPoints;
    public Awarness awarness = Awarness.PATROLLING;

    private int patrolPointIndex;
    public int raycastNumber = 2;
    public float fieldOfView = 90f;
    public float distanceOfView = 10f;

    public string preyId;
    private GameObject prey;

    void Start() {

    }

    void Update() {
        switch(awarness) {
            case Awarness.PATROLLING:
                UpdatePatrol();
                UpdateLooking();
                break;
            case Awarness.CHASING:
                UpdateChase();
                break;
        }
    }

    void UpdateChase() {

    }

    void UpdatePatrol() {
        if(patrolPoints.Length > 0) {
            MoveToward(patrolPoints[patrolPointIndex]);
            if(IsNearPoint(patrolPoints[patrolPointIndex], 5f)) {
                patrolPointIndex = (patrolPointIndex + 1) % patrolPoints.Length;
            }
        }
    }

    void UpdateLooking() {
        RaycastHit[] hits;
        Vector3 pos = new Vector3(Position.x, 1f, Position.z);
        Debug.DrawLine(pos, pos + new Vector3(velocity.x, 1f, velocity.y).normalized * distanceOfView, Color.red);

        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg - fieldOfView / 2f;
        for (int i = 0; i < raycastNumber; i++) {
            Vector3 cartAngle = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0f, Mathf.Sin(angle * Mathf.Deg2Rad));
            Debug.DrawLine(pos, pos + cartAngle * distanceOfView, Color.green);
            hits = Physics.RaycastAll(pos, cartAngle, distanceOfView);
            angle += fieldOfView / (float)(raycastNumber - 1);

            GameObject found = FindTarget(hits, preyId);
            if (found != null) {
                Debug.Log("A MOI ! " + found.name);
            }
        }
    }

    private GameObject FindTarget(RaycastHit[] hits, string entityId) {
        for (int i = 0; i < hits.Length; i++) {
            GameObject collide = hits[i].collider.gameObject;
            if (collide.GetComponent<Entity>() != null) {
                Entity entity = collide.GetComponent<Entity>();
                if (collide.tag == "Hide") {
                    if (String.Compare(entity.entityId, hideId) != 0) {
                        return null;
                    }
                } else if (String.Compare(entity.entityId, entityId) == 0) {
                    return collide;
                }
            } else {
                return null;
            }
        }
        return null;
    }
}
