﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HyenaEntity : AnimalEntity
{
    public enum Awarness {
        SLEEPING = 0, STANDING, PATROLLING, CHASING, SUSPICIOUS
    }

    [Serializable]
    public struct MovementsValues {
        public float speedMax;
        //public MovementCurve speed;
        public MovementCurve acceleration;
        public MovementCurve frictions;
        public MovementCurve turnAround;
        public MovementCurve turn;

        [Range(0f, 90f)] public float turnAngleMin;
        [Range(0f, 90f)] public float turnAngleMax;
        public float turnDurationMin;
        public float turnDurationMax;
    }

    [Header("Hyena")]
    public Awarness awarness = Awarness.PATROLLING;

    [Header("Field of view")]
    public int raycastNumber = 2;
    public float fieldOfView = 90f;
    public float distanceOfView = 10f;

    [Header("Patrolling")]
    public Vector2[] patrolPoints;
    [SerializeField] private MovementsValues patrolValues = new MovementsValues();
    private int patrolPointIndex;

    [Header("Chase")]
    public string preyId;
    //public float chaseSpeedFactor = 1.5f;
    [SerializeField] private MovementsValues chaseValues = new MovementsValues();
    private GameObject prey;

    [Header("Suspicious")]
    public float searchTime = 2f;
    public float timeBeforeChase = 2f;
    [SerializeField] private MovementsValues suspiciousValues = new MovementsValues();
    private float searchCountdown = -1f;
    private float beforeChaseCountdown = -1f;
    private Vector2 lastPreyPos = Vector2.zero;

    #region Properties

    public bool HasPrey {
        get { return prey != null; }
        private set {}
    }

    #endregion

    #region Unity callbacks

    protected override void Start() {
        base.Start();
        beforeChaseCountdown = timeBeforeChase;
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

    #endregion

    #region Movements

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
            if (beforeChaseCountdown > 0f) {
                beforeChaseCountdown -= Time.deltaTime;
            } else {
                Chase(targ);
                return;
            }
        } else {
            searchCountdown -= Time.deltaTime;
            if (searchCountdown <= 0) {
                Patrol();
            }
        }
    }

    #endregion

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
                    if (!hideId.Equals(entity.entityId)) {
                        return null;
                    }
                } else if (entityId.Equals(entity.entityId)) {
                    return collide;
                }
            } else {
                return null;
            }
        }
        return null;
    }

    public void Patrol() {
        CopyMovementsValues(patrolValues);

        prey = null;
        awarness = Awarness.PATROLLING;
    }

    public void Chase(GameObject targ) {
        prey = targ;
        Follow(prey);

        CopyMovementsValues(chaseValues);

        awarness = Awarness.CHASING;
    }

    public void Search(Vector2 pos) {
        ClearFollow();
        prey = null;
        MoveToDestination(pos.ConvertTo3D());
        searchCountdown = searchTime;
        lastPreyPos = pos;

        CopyMovementsValues(suspiciousValues);

        awarness = Awarness.SUSPICIOUS;
    }

    public void CopyMovementsValues(MovementsValues values) {
        speedMax = values.speedMax;
        acceleration = values.acceleration;
        frictions = values.frictions;
        turn = values.turn;
        turnAround = values.turnAround;

        turnAngleMin = values.turnAngleMin;
        turnAngleMax = values.turnAngleMax;
        turnDurationMin = values.turnDurationMin;
        turnDurationMax = values.turnDurationMax;
    }
}
