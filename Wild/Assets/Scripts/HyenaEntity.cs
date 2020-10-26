using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HyenaEntity : AnimalEntity
{
    public enum Awarness {
        PATROLLING, CHASING, SUSPICIOUS
    }

    [Header("Hyena")]
    public Awarness awarness = Awarness.PATROLLING;

    [Header("Field of view")]
    public int raycastNumber = 2;
    public float fieldOfView = 90f;
    public float distanceOfView = 10f;

    [Header("Patrolling")]
    public Vector2[] patrolPoints;
    private int patrolPointIndex;

    [Header("Chase")]
    public string preyId;
    public float chaseSpeedFactor = 1.5f;
    private GameObject prey;

    [Header("Suspicious")]
    public float searchTime = 2f;
    private float searchCountdown = -1f;
    private Vector2 lastPreyPos = Vector2.zero;

    protected override void Start() {
        base.Start();
    }

    void Update() {
        switch (awarness) {
            case Awarness.PATROLLING:
                UpdatePatrol();
                break;
            case Awarness.CHASING:
                UpdateChase();
                break;
            case Awarness.SUSPICIOUS:
                UpdateSearch();
                break;
        }
    }

    void UpdatePatrol() {
        if(patrolPoints.Length > 0) {
            //MoveToDestination(new Vector3(patrolPoints[patrolPointIndex].x, 0, patrolPoints[patrolPointIndex].y));
            MoveToDestination(patrolPoints[patrolPointIndex].ConvertTo3D());
            if (IsNearPoint(patrolPoints[patrolPointIndex], destinationRadius)) {
                patrolPointIndex = (patrolPointIndex + 1) % patrolPoints.Length;
            }
        }

        GameObject targ = Looking();
        if (targ != null) {
            Chase(targ);
        }
    }

    void UpdateChase() {
        GameObject targ = Looking();
        if (targ == null) {
            //Search(new Vector3(prey.transform.position.x, prey.transform.position.z));
            Search(prey.transform.position.ConvertTo2D());
        }
    }

    void UpdateSearch() {
        GameObject targ = Looking();
        if (targ != null) {
            Chase(targ);
            return;
        }

        if(IsNearPoint(lastPreyPos, destinationRadius)) {
            searchCountdown -= Time.deltaTime;
            if(searchCountdown <= 0) {
                patrol();
            }
        }
    }

    GameObject Looking() {
        RaycastHit[] hits;
        Vector3 pos = new Vector3(Position.x, 1f, Position.z);
        //Debug.DrawLine(pos, pos + new Vector3(direction.x, 1f, direction.y).normalized * distanceOfView, Color.red);
        Debug.DrawLine(pos, pos + direction.ConvertTo3D().normalized * distanceOfView, Color.red);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - fieldOfView / 2f;
        for (int i = 0; i < raycastNumber; i++) {
            Vector3 cartAngle = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0f, Mathf.Sin(angle * Mathf.Deg2Rad));
            Debug.DrawLine(pos, pos + cartAngle * distanceOfView, Color.green);
            hits = Physics.RaycastAll(pos, cartAngle, distanceOfView);
            angle += fieldOfView / (float)(raycastNumber - 1);

            GameObject found = FindTarget(hits, preyId);
            if (found != null) {
                return found;
            }
        }

        return null;
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

    private void patrol() {
        if (awarness == Awarness.CHASING) {
            MultMovements(1f / chaseSpeedFactor);
        }
        awarness = Awarness.PATROLLING;
    }

    private void Chase(GameObject targ) {
        prey = targ;
        Follow(prey);

        if(awarness != Awarness.CHASING) {
            MultMovements(chaseSpeedFactor);
        }
        awarness = Awarness.CHASING;
    }

    private void Search(Vector2 pos) {
        ClearFollow();
        prey = null;
        MoveToDestination(pos.ConvertTo3D());
        searchCountdown = searchTime;
        lastPreyPos = pos;

        if (awarness == Awarness.CHASING){
            MultMovements(1f / chaseSpeedFactor);
        }
        awarness = Awarness.SUSPICIOUS;
    }

    private void MultMovements(float factor) {
        speedMax *= factor;
        acceleration *= factor;
        friction /= factor;
        turnFriction /= factor;
    }
}
