using System;
using UnityEngine;

public class HyenaEntity : AnimalEntity
{
    public enum Awarness
    {
        PATROLLING = 0, SLEEPING, STANDING, CHASING, SUSPICIOUS
    }

    [Serializable]
    public struct MovementsValues
    {
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
    private Animator animator;

    public bool debug = false;

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
        private set { }
    }

    #endregion

    #region Unity callbacks

    protected override void Start()
    {
        base.Start();
        beforeChaseCountdown = timeBeforeChase;
        animator = GetComponentInChildren<Animator>();

        float orientAngle = Mathf.Atan2(OrientDir.y, OrientDir.x);
        orientAngle = Mathf.Sign(orientAngle) * (Mathf.Abs(orientAngle) % (Mathf.PI * 2f));
        _lastSmoothOrientDir = orientDir;
    }

    private float _orientTimer = 0f;

    void Update()
    {
        UpdateAnims();
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

    [Header("Animation")]
    private float _animWalkSpeedMin = 0.1f;

    //Smooth orient
    //(!) You can increase or decrease orient duration to slowdown entity anim smooth turns
    private float ANIM_SMOOTH_ORIENT_DURATION = 0.075f;

    public const float ANIM_ANGLE_STEP = Mathf.PI / 8f;
    private bool _smoothOrientEnabled = false;
    private int _smoothOrientStepDelta = 0;
    private float _smoothOrientTimer = 0f;

    private Vector2 _lastSmoothOrientDir = Vector2.zero;

    void UpdateAnims()
    {
        switch (awarness) {
            case Awarness.PATROLLING:
            case Awarness.STANDING:
            case Awarness.SUSPICIOUS:
                if (velocity.magnitude > _animWalkSpeedMin) {
                    animator.SetBool("Running", false);
                    animator.SetBool("Walking", true);
                } else {
                    animator.SetBool("Running", false);
                    animator.SetBool("Walking", false);
                }
                break;

            case Awarness.CHASING:
                animator.SetBool("Running", true);
                animator.SetBool("Walking", false);
                break;
        }

        animator.SetBool("Sleeping", awarness == Awarness.SLEEPING);

        //Calculate difference between orientDir and last smooth orient dir
        float orientAngleDiff = Vector2.SignedAngle(_lastSmoothOrientDir, orientDir) * Mathf.Deg2Rad;
        //Normalize difference using ANIM_ANGLE STEP (Here there are 8 orient per anim) so we need to divide by PI / 8
        int orientStepDiff = (int)Mathf.Sign(orientAngleDiff) * Mathf.FloorToInt(Mathf.Abs(orientAngleDiff) / ANIM_ANGLE_STEP);
        if (orientStepDiff != 0) {
            //Increment a step delta (will be used later) and enable smooth orientation system
            _smoothOrientStepDelta += orientStepDiff;
            if (_smoothOrientStepDelta != 0) {
                if (!_smoothOrientEnabled) {
                    _smoothOrientTimer = 0f;
                }
                _smoothOrientEnabled = true;
            }
            _lastSmoothOrientDir = orientDir;
        }

        if (_smoothOrientEnabled) {
            //For each step available :
            //Interpolate between 0f and PI/8 (multiplied by factor to orient angle)
            //Using a small duration to manage interpolate (better if < 0.1f)
            _smoothOrientTimer += Time.deltaTime;
            float ratio = _smoothOrientTimer / ANIM_SMOOTH_ORIENT_DURATION;
            float smoothOrientAngleEnd = Mathf.Sign(_smoothOrientStepDelta) * ANIM_ANGLE_STEP;
            float smoothOrientAngle = Mathf.Lerp(
                0f,
                smoothOrientAngleEnd,
                ratio
            );

            if (_smoothOrientTimer >= ANIM_SMOOTH_ORIENT_DURATION) {
                //If interpolation is finished => move to next step
                _smoothOrientTimer -= ANIM_SMOOTH_ORIENT_DURATION;
                _smoothOrientStepDelta -= (int)Mathf.Sign(_smoothOrientStepDelta);

                if (_smoothOrientStepDelta != 0) {
                    //Update animation orientation using orientDir and step delta
                    float orientAngle = Mathf.Atan2(orientDir.y, orientDir.x);
                    float intermediateOrientAngle = orientAngle + (_smoothOrientStepDelta) * ANIM_ANGLE_STEP;
                    animator.SetFloat("MoveX", Mathf.Cos(intermediateOrientAngle));
                    animator.SetFloat("MoveY", Mathf.Sin(intermediateOrientAngle));

                    //Reset smooth orient angle
                    smoothOrientAngle = 0f;
                } else {
                    //If there is no step => entity will be oriented using orient dir directly
                    animator.SetFloat("MoveX", OrientDir.x);
                    animator.SetFloat("MoveY", OrientDir.y);
                    smoothOrientAngle = 0f;

                    //disable smooth orientation system
                    _smoothOrientEnabled = false;
                }
            }

            Vector3 localEulerAngles = animator.transform.localEulerAngles;
            localEulerAngles.z = -smoothOrientAngle * Mathf.Rad2Deg;
            animator.transform.localEulerAngles = localEulerAngles;

        }

    }

    void UpdatePatrol()
    {
        if (patrolPoints.Length > 0) {
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

    void UpdateChase()
    {
        GameObject targ = Looking();

        if (targ == null) {
            //Search(new Vector3(prey.transform.position.x, prey.transform.position.z));
            Search(prey.transform.position.ConvertTo2D());
        }
    }

    void UpdateSearch()
    {
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

    GameObject Looking()
    {
        RaycastHit[] hits;
        Vector3 pos = new Vector3(Position.x, 1f, Position.z);
        //Debug.DrawLine(pos, pos + new Vector3(direction.x, 1f, direction.y).normalized * distanceOfView, Color.red);
        //Debug.DrawLine(pos, pos + direction.ConvertTo3D().normalized * distanceOfView, Color.red);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - fieldOfView / 2f;
        for (int i = 0; i < raycastNumber; i++) {
            Vector3 cartAngle = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0f, Mathf.Sin(angle * Mathf.Deg2Rad));
            //Debug.DrawLine(pos, pos + cartAngle * distanceOfView, Color.green);
            hits = Physics.RaycastAll(pos, cartAngle, distanceOfView);
            angle += fieldOfView / (float)(raycastNumber - 1);

            GameObject found = FindTarget(hits, preyId);
            for (int j = 0; j < hits.Length; j++) {
                //if (debug) { Debug.Log(j + "- " + hits[j].collider); }
            }

            if (found != null) {
                return found;
            }
        }

        return null;
    }

    private GameObject FindTarget(RaycastHit[] hits, string entityId)
    {
        if (hits.Length <= 0) { return null; }

        //List<RaycastHit> obj = new List<RaycastHit>();
        //obj.Add(hits[0]);
        //float nearest = obj[0].distance;
        //for (int i = 0; i < hits.Length; i++) {
        //    if(nearest > obj[i].distance) {
        //        obj.Insert(0, );
        //    }
        //}

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

    public void Patrol()
    {
        CopyMovementsValues(patrolValues);

        prey = null;
        awarness = Awarness.PATROLLING;
    }

    public void Chase(GameObject targ)
    {
        prey = targ;
        Follow(prey);

        CopyMovementsValues(chaseValues);

        awarness = Awarness.CHASING;
    }

    public void Search(Vector2 pos)
    {
        ClearFollow();
        prey = null;
        MoveToDestination(pos.ConvertTo3D());
        searchCountdown = searchTime;
        lastPreyPos = pos;

        CopyMovementsValues(suspiciousValues);

        awarness = Awarness.SUSPICIOUS;
    }

    public void CopyMovementsValues(MovementsValues values)
    {
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
