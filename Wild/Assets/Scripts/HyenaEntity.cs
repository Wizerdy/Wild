using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoundManager;

public class HyenaEntity : AnimalEntity {
    public enum Awarness {
        PATROLLING = 0, SLEEPING, STANDING, CHASING, SUSPICIOUS
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

    private Awarness startAwarness = Awarness.PATROLLING;
    private Vector3 startPosition = Vector3.zero;
    private Quaternion startRotation = Quaternion.identity;

    private Animator animator;

    public bool debug = false;

    [Header("Field of view")]
    public int raycastNumber = 2;
    public float fieldOfView = 90f;
    public float distanceOfView = 10f;

    public float presenceRadius = 15f;

    //private Vector3[] periphericalPoints;
    private float[] visionAngles;

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


    public float suspiciousFactor = 0f;
    private float suspiciousSpeedFactor = 1f;

    [Header("Sounds")]
    [SerializeField] private QueuedSoundObject laughs = null;

    #region Properties

    public bool HasPrey {
        get { return prey != null; }
        private set { }
    }

    public bool Suspicious {
        get { return (suspiciousFactor >= 1f ? true : false); }
        private set { }
    }

    #endregion

    #region Unity callbacks

    protected override void Start() {
        base.Start();
        beforeChaseCountdown = timeBeforeChase;
        animator = GetComponentInChildren<Animator>();

        startAwarness = awarness;
        startPosition = transform.position;
        startRotation = transform.rotation;

        FindVisionPoints();
    }

    void Update() {
        GameObject prey = FeelPresence(preyId);
        if (prey != null) {
            BeSuspicious(2f);

            if (Suspicious) {
                Chase(prey);
            }
        }

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
            case Awarness.STANDING:
                UpdateStanding();
                break;
        }

        UpdateSuspicious();
    }

    #endregion

    #region Updates

    void UpdateStanding() {
        GameObject targ = Looking();
        if (targ != null) {
            Chase(targ);
        } else {
            targ = PeripheralLooking();

            if (targ != null) {
                BeSuspicious();
                if (Suspicious) {
                    Chase(targ);
                }
            }
        }
    }

    void UpdatePatrol() {
        if (patrolPoints.Length > 0) {
            //MoveToDestination(new Vector3(patrolPoints[patrolPointIndex].x, 0, patrolPoints[patrolPointIndex].y));
            if (patrolPoints.Length > 1 || !IsNearPoint(patrolPoints[patrolPointIndex], destinationRadius)) {
                animator.SetBool("Running", false);
                animator.SetBool("Walking", true);
                animator.SetFloat("MoveX", -(transform.position.ConvertTo2D() - patrolPoints[patrolPointIndex]).normalized.x);
                animator.SetFloat("MoveY", -(transform.position.ConvertTo2D() - patrolPoints[patrolPointIndex]).normalized.y);
            } else {
                animator.SetBool("Running", false);
                animator.SetBool("Walking", false);
                animator.SetFloat("MoveX", -(transform.position.ConvertTo2D() - patrolPoints[patrolPointIndex]).normalized.x);
                animator.SetFloat("MoveY", -(transform.position.ConvertTo2D() - patrolPoints[patrolPointIndex]).normalized.y);
            }

            MoveToDestination(patrolPoints[patrolPointIndex].ConvertTo3D());
            if (IsNearPoint(patrolPoints[patrolPointIndex], destinationRadius)) {
                patrolPointIndex = (patrolPointIndex + 1) % patrolPoints.Length;
            }
        }

        GameObject targ = Looking();
        if (targ != null) {
            Chase(targ);
        } else {
            targ = PeripheralLooking();

            if (targ != null) {
                BeSuspicious();
                if (Suspicious) {
                    Chase(targ);
                }
            }
        }
    }

    void UpdateChase() {
        GameObject targ = Looking();
        if (targ == null) {
            //Search(new Vector3(prey.transform.position.x, prey.transform.position.z));
            Search(prey.transform.position.ConvertTo2D());
            return;
        } else {

            targ = PeripheralLooking();

            if (targ == null) {
                Search(prey.transform.position.ConvertTo2D());
                return;
            }

            animator.SetBool("Running", true);
            animator.SetBool("Walking", false);

            animator.SetFloat("MoveX", -(transform.position.ConvertTo2D() - targ.transform.position.ConvertTo2D()).normalized.x);
            animator.SetFloat("MoveY", -(transform.position.ConvertTo2D() - targ.transform.position.ConvertTo2D()).normalized.y);
        }

        BeSuspicious();
    }

    void UpdateSearch() {
        GameObject targ = Looking();

        if (targ != null) {
            Chase(targ);
            return;
        } else {
            targ = PeripheralLooking();
            if (targ != null) {
                BeSuspicious();
                if (Suspicious) {
                    Chase(targ);
                }
            } else {
                searchCountdown -= Time.deltaTime;
                if (searchCountdown <= 0) {
                    Patrol();
                }
            }
        }
    }

    #endregion

    #region Detection

    void FindVisionPoints() {
        if (raycastNumber < 2) { Debug.LogError("Not Enough raycasts"); return; }

        visionAngles = new float[2];
        float angle = -fieldOfView / 2f;
        Vector3[] points = new Vector3[2];
        for (int i = 0; i < 2; i++) {
            Vector3 cartAng = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0f, Mathf.Sin(angle * Mathf.Deg2Rad));
            points[i] = cartAng * (distanceOfView / 3f);
            points[i] = points[i] + new Vector3(distanceOfView / 3f * 2f, 0f, 0f);
            angle += fieldOfView / (float)(raycastNumber - 1);
        }

        visionAngles[0] = Mathf.Atan2(points[0].z, points[0].x) * Mathf.Rad2Deg;
        visionAngles[1] = Mathf.Atan2(points[1].z, points[1].x) * Mathf.Rad2Deg - visionAngles[0];
    }

    GameObject PeripheralLooking() {
        List<RaycastHit> hits = new List<RaycastHit>();
        Vector3 pos = new Vector3(Position.x, 1f, Position.z);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - fieldOfView / 2f;
        for (int i = 0; i < raycastNumber; i++) {
            Vector3 cartAngle = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0f, Mathf.Sin(angle * Mathf.Deg2Rad));
            Vector3 advancedPos = pos + cartAngle * distanceOfView / 3f;
            Debug.DrawLine(pos, pos + cartAngle * distanceOfView / 3f, Color.yellow);
            Debug.DrawLine(advancedPos, advancedPos + (direction.normalized * distanceOfView / 3f * 2f).ConvertTo3D(1f), Color.yellow);
            hits.AddRange(Physics.RaycastAll(pos, cartAngle, distanceOfView / 3f));
            hits.AddRange(Physics.RaycastAll(advancedPos, direction.ConvertTo3D(), distanceOfView / 3f * 2f));

            GameObject found = FindTarget(hits.ToArray(), preyId);

            if (found != null) {
                return found;
            }

            angle += fieldOfView / (float)(raycastNumber - 1);
        }

        return null;
    }

    GameObject Looking() {
        RaycastHit[] hits;
        Vector3 pos = new Vector3(Position.x, 1f, Position.z);
        //Debug.DrawLine(pos, pos + new Vector3(direction.x, 1f, direction.y).normalized * distanceOfView, Color.red);
        Debug.DrawLine(pos, pos + direction.ConvertTo3D().normalized * distanceOfView, Color.blue);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + visionAngles[0];
        for (int i = 0; i < raycastNumber; i++) {
            Vector3 cartAngle = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0f, Mathf.Sin(angle * Mathf.Deg2Rad));
            Debug.DrawLine(pos, pos + cartAngle * distanceOfView, Color.red);
            hits = Physics.RaycastAll(pos, cartAngle, distanceOfView);
            angle += visionAngles[1];

            GameObject found = FindTarget(hits, preyId);

            if (found != null) {
                return found;
            }
        }

        return null;
    }

    private GameObject FindTarget(RaycastHit[] hits, string entityId) {
        if (hits.Length <= 0) { return null; }

        bool swapped = true;
        while (swapped) {
            swapped = false;
            for (int i = 0; i < hits.Length - 1; i++) {
                if (hits[i].distance > hits[i + 1].distance) {
                    RaycastHit hit = hits[i];
                    hits[i] = hits[i + 1];
                    hits[i + 1] = hit;
                    swapped = true;
                }
            }
        }

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

    private GameObject FeelPresence(string preyId) {
        Collider[] colliders = Physics.OverlapSphere(transform.position, presenceRadius);
        for (int i = 0; i < colliders.Length; i++) {
            Entity entity = colliders[i].gameObject.GetComponent<Entity>();
            if (entity != null && entity.IsEntityId(preyId)) {
                return entity.gameObject;
            }
        }
        return null;
    }

    #endregion

    #region States

    public void Patrol() {
        CopyMovementsValues(patrolValues);

        prey = null;
        awarness = Awarness.PATROLLING;

        Laugh((int)awarness);
    }

    public void Chase(GameObject targ) {
        prey = targ;
        Follow(prey);

        CopyMovementsValues(chaseValues);

        suspiciousFactor = 1f;

        awarness = Awarness.CHASING;

        Laugh((int)awarness);
    }

    public void Search(Vector2 pos) {
        ClearFollow();
        prey = null;
        MoveToDestination(pos.ConvertTo3D());
        searchCountdown = searchTime;
        lastPreyPos = pos;

        CopyMovementsValues(suspiciousValues);

        awarness = Awarness.SUSPICIOUS;

        Laugh((int)awarness);
    }

    #endregion

    public void Laugh(int index) {
        if (laughs == null) { return; }
        laughs.Play(index);
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

    private void BeSuspicious(float factor = 1f) {
        suspiciousSpeedFactor = factor;
    }

    private void UpdateSuspicious() {
        if (timeBeforeChase == 0f) { suspiciousFactor = 1f; return; }

        if (suspiciousSpeedFactor > 0f) {
            if (suspiciousFactor < 1f) {
                suspiciousFactor += Time.deltaTime / timeBeforeChase * suspiciousSpeedFactor;
            } else {
                suspiciousFactor = 1f;
            }
        } else if (suspiciousFactor > 0f) {
            suspiciousFactor -= Time.deltaTime / timeBeforeChase;
        }

        if (suspiciousFactor < 0f) {
            suspiciousFactor = 0f;
        }

        suspiciousSpeedFactor = 0f;
    }

    public void ResetToStart() {
        transform.position = startPosition;
        transform.rotation = startRotation;
        awarness = startAwarness;
    }
}
