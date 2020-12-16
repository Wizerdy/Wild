using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoundManager;

[SelectionBase]
public class HyenaEntity : AnimalEntity {
    public enum Awarness {
        PATROLLING = 0, SLEEPING, STANDING, CHASING, SUSPICIOUS, RETURNING
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
    public Color circleColor;

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
    private bool destinationReached = false;

    [Header("Sleeping")]
    [Range(-1.0f, 1.0f)] public float moveX;
    [Range(-1.0f, 1.0f)] public float moveY;

    public float suspiciousFactor = 0f;
    private float suspiciousSpeedFactor = 0f;

    public bool wereSuspicious = false;

    [Header("Animation")]
    private float _animWalkSpeedMin = 0.2f;
    public const float ANIM_ANGLE_STEP = Mathf.PI / 8f;
    private float ANIM_SMOOTH_ORIENT_DURATION = 0.075f;

    private bool _smoothOrientEnabled = false;
    private int _smoothOrientStepDelta = 0;
    private float _smoothOrientTimer = 0f;
    private float _orientTimer = 0f;

    private Vector2 _lastSmoothOrientDir = Vector2.zero;

    private ParticleSystem[] fxs = null;
    private int fxIndex = -1;
    

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

        float orientAngle = Mathf.Atan2(OrientDir.y, OrientDir.x);
        orientAngle = Mathf.Sign(orientAngle) * (Mathf.Abs(orientAngle) % (Mathf.PI * 2f));
        _lastSmoothOrientDir = orientDir;

        Transform fx = transform.Find("FX");
        if (fx != null)
        {
            fxs = new ParticleSystem[fx.childCount];
            for (int i = 0; i < fx.childCount; i++)
            {
                fxs[i] = fx.GetChild(i).GetComponent<ParticleSystem>();
                Debug.Log(fxs[i].name);
            }
        }

        ChangeState(awarness);
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
            case Awarness.STANDING:
                UpdateStanding();
                break;
            case Awarness.RETURNING:
                UpdateReturn();
                break;
        }

        // Aura of detection
        if (awarness != Awarness.CHASING) {
            GameObject prey = FeelPresence(preyId);
            if (prey != null) {
                BeSuspicious(3f);

                if (Suspicious) {
                    ChangeState(Awarness.CHASING, prey);
                }
            }
        }

        UpdateSuspicious();
        UpdateAnims();
    }

    #endregion

    #region Updates

    void UpdateAnims() {
        switch (awarness) {
            case Awarness.RETURNING:
            case Awarness.PATROLLING:
            case Awarness.STANDING:
            case Awarness.SUSPICIOUS:
            case Awarness.SLEEPING:
                if (velocity.sqrMagnitude > _animWalkSpeedMin * _animWalkSpeedMin) {
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
        if (awarness == Awarness.SLEEPING)
        {
            animator.SetFloat("MoveX", moveX);
            animator.SetFloat("MoveY", moveY);
        }
        else
        {
            animator.SetFloat("MoveX", OrientDir.x);
            animator.SetFloat("MoveY", OrientDir.y);
        }

        #region Angled animation

        ////Calculate difference between orientDir and last smooth orient dir
        //float orientAngleDiff = Vector2.SignedAngle(_lastSmoothOrientDir, orientDir) * Mathf.Deg2Rad;
        ////Normalize difference using ANIM_ANGLE STEP (Here there are 8 orient per anim) so we need to divide by PI / 8
        //int orientStepDiff = (int)Mathf.Sign(orientAngleDiff) * Mathf.FloorToInt(Mathf.Abs(orientAngleDiff) / ANIM_ANGLE_STEP);
        //if (orientStepDiff != 0) {
        //    //Increment a step delta (will be used later) and enable smooth orientation system
        //    _smoothOrientStepDelta += orientStepDiff;
        //    if (_smoothOrientStepDelta != 0) {
        //        if (!_smoothOrientEnabled) {
        //            _smoothOrientTimer = 0f;
        //        }
        //        _smoothOrientEnabled = true;
        //    }
        //    _lastSmoothOrientDir = orientDir;
        //}

        //if (_smoothOrientEnabled) {
        //    //For each step available :
        //    //Interpolate between 0f and PI/8 (multiplied by factor to orient angle)
        //    //Using a small duration to manage interpolate (better if < 0.1f)
        //    _smoothOrientTimer += Time.deltaTime;
        //    float ratio = _smoothOrientTimer / ANIM_SMOOTH_ORIENT_DURATION;
        //    float smoothOrientAngleEnd = Mathf.Sign(_smoothOrientStepDelta) * ANIM_ANGLE_STEP;
        //    float smoothOrientAngle = Mathf.Lerp(
        //        0f,
        //        smoothOrientAngleEnd,
        //        ratio
        //    );

        //    if (_smoothOrientTimer >= ANIM_SMOOTH_ORIENT_DURATION) {
        //        //If interpolation is finished => move to next step
        //        _smoothOrientTimer -= ANIM_SMOOTH_ORIENT_DURATION;
        //        _smoothOrientStepDelta -= (int)Mathf.Sign(_smoothOrientStepDelta);

        //        if (_smoothOrientStepDelta != 0) {
        //            //Update animation orientation using orientDir and step delta
        //            float orientAngle = Mathf.Atan2(orientDir.y, orientDir.x);
        //            float intermediateOrientAngle = orientAngle + (_smoothOrientStepDelta) * ANIM_ANGLE_STEP;
        //            animator.SetFloat("MoveX", Mathf.Cos(intermediateOrientAngle));
        //            animator.SetFloat("MoveY", Mathf.Sin(intermediateOrientAngle));

        //            //Reset smooth orient angle
        //            smoothOrientAngle = 0f;
        //        } else {
        //            //If there is no step => entity will be oriented using orient dir directly
        //            animator.SetFloat("MoveX", OrientDir.x);
        //            animator.SetFloat("MoveY", OrientDir.y);
        //            smoothOrientAngle = 0f;

        //            //disable smooth orientation system
        //            _smoothOrientEnabled = false;
        //        }
        //    }

        //    Vector3 localEulerAngles = animator.transform.localEulerAngles;
        //    localEulerAngles.z = -smoothOrientAngle * Mathf.Rad2Deg;
        //    animator.transform.localEulerAngles = localEulerAngles;
        //}

        #endregion
    }

    void UpdateStanding() {
        GameObject targ = Watch();

        if (targ != null) {
            ChangeState(Awarness.CHASING, targ);
        }
    }

    void UpdatePatrol() {
        GameObject targ = Watch();
        if (targ != null) {
            ChangeState(Awarness.CHASING, targ);
            return;
        }

        if (patrolPoints.Length > 0) {
            //MoveToDestination(new Vector3(patrolPoints[patrolPointIndex].x, 0, patrolPoints[patrolPointIndex].y));

            MoveToDestination(patrolPoints[patrolPointIndex].ConvertTo3D());
            if (IsNearPoint(patrolPoints[patrolPointIndex], destinationRadius)) {
                patrolPointIndex = (patrolPointIndex + 1) % patrolPoints.Length;
            }
        }
    }

    void UpdateChase() {
        GameObject targ = Watch();
        if (targ == null) {
            //Search(prey.transform.position.ConvertTo2D());
            ChangeState(Awarness.SUSPICIOUS, prey);
            return;
        }

        //Debug.Log(targ);
        BeSuspicious();
    }

    void UpdateSearch() {
        GameObject targ = Watch();
        if (targ != null) {
            ChangeState(Awarness.CHASING, targ);
            return;
        } else {
            if (destinationReached || IsNearPoint(lastPreyPos, destinationRadius)) {
                destinationReached = true;

                searchCountdown -= Time.deltaTime;
                if (searchCountdown <= 0) {
                    Return();
                }
            }
        }
    }

    void UpdateReturn() {
        GameObject targ = Watch();

        if (targ != null) {
            ChangeState(Awarness.CHASING, targ);
            return;
        }

        if (IsNearPoint(startPosition.ConvertTo2D(), destinationRadius)) {
            ResetToStart(false);
        } else {
            MoveToDestination(startPosition.ConvertTo2D());
        }
    }

    private void UpdateSuspicious() {
        if (timeBeforeChase == 0f) { suspiciousFactor = 1f; return; }

        if (suspiciousSpeedFactor > 0f) {
            if (!wereSuspicious && awarness != Awarness.CHASING) { StopFx(2); PlayFx(1); }

            if (suspiciousFactor < 1f) {
                suspiciousFactor += Time.deltaTime / timeBeforeChase * suspiciousSpeedFactor;
            } else {
                suspiciousFactor = 1f;
            }
            wereSuspicious = true;
        } else if (suspiciousFactor > 0f) {
            suspiciousFactor -= Time.deltaTime / timeBeforeChase;
            wereSuspicious = false;
        }

        if (suspiciousFactor < 0f) {
            suspiciousFactor = 0f;
        }

        suspiciousSpeedFactor = 0f;
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

    GameObject Watch() {
        GameObject targ = Looking();

        if (targ != null) {
            return targ;
        } else {
            targ = PeripheralLooking();
            if (targ != null) {
                BeSuspicious();
                if (Suspicious) {
                    return targ;
                }
            }
        }

        return null;
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
        Debug.DrawLine(pos, pos + direction.ConvertTo3D().normalized * distanceOfView, new Color(255, 125, 0));

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

    public void ChangeState(Awarness awarness, GameObject prey = null) {
        if (this.awarness == Awarness.SLEEPING) {
            StopFx(0);
        }

        if (this.awarness == Awarness.CHASING && awarness != Awarness.CHASING)
        {
            GameManager.Instance.IncrementChasingHyenas(-1);
        }

        switch (awarness) {
            case Awarness.PATROLLING:
                Patrol();
                break;
            case Awarness.CHASING:
                if(prey == null) { return; }
                Chase(prey);
                break;
            case Awarness.SUSPICIOUS:
                if (prey == null) { return; }
                Search(prey.transform.position.ConvertTo2D());
                break;
            case Awarness.RETURNING:
                Return();
                break;
            case Awarness.SLEEPING:
                PlayFx(0);
                this.awarness = awarness;
                break;
            default:
                this.awarness = awarness;
                break;
        }
    }

    public void Patrol() {
        if (debug) { Debug.LogWarning("Patrolling"); }

        CopyMovementsValues(patrolValues);

        prey = null;
        awarness = Awarness.PATROLLING;

        Laugh((int)awarness);
    }

    public void Chase(GameObject targ) {
        if (debug) { Debug.LogWarning("Chasing"); }

        StopFx(1);
        PlayFx(2);

        prey = targ;
        Follow(prey);

        CopyMovementsValues(chaseValues);

        suspiciousFactor = 1f;

        awarness = Awarness.CHASING;

        GameManager.Instance.IncrementChasingHyenas(1);

        Laugh((int)awarness);

        SoundManager.SoundManager.instance.PlayMusic(1);
    }

    public void Search(Vector2 pos) {
        if (debug) { Debug.LogWarning("Searching"); }

        StopFx(2);
        PlayFx(1);

        ClearFollow();
        prey = null;
        MoveToDestination(pos.ConvertTo3D());
        searchCountdown = searchTime;
        destinationReached = false;
        lastPreyPos = pos;

        CopyMovementsValues(suspiciousValues);

        awarness = Awarness.SUSPICIOUS;

        SoundManager.SoundManager.instance.PlayMusic(2);

        Laugh((int)awarness);
    }

    public void Return() {
        switch (startAwarness) {
            case Awarness.PATROLLING:
                ChangeState(Awarness.PATROLLING);
                break;
            case Awarness.SLEEPING:
            case Awarness.STANDING:
                awarness = Awarness.RETURNING;
                MoveToDestination(startPosition);
                break;
            default:
                break;
        }
    }

    #endregion

    #region FX

    void PlayFx(int index)
    {
        if (fxs == null) { return; }

        if (index > fxs.Length || index < 0)
        {
            return;
        }

        fxs[index].Play();
        fxIndex = index;
    }

    void StopFxs()
    {
        if (fxs == null) { return; }

        for (int i = 0; i < fxs.Length; i++)
        {
            StopFx(i);
        }
    }

    void StopFx(int index)
    {
        if (fxs == null) { return; }

        if (index < 0 || !fxs[index].isPlaying) { return; }

        fxs[index].Stop();
        fxs[index].Clear();

        if (fxs[index].particleCount > 0)
        {
            fxs[index].Clear();
        }
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

    public void ResetToStart(bool resetPos = true) {
        if (resetPos) { transform.position = startPosition; }
        transform.rotation = startRotation;
        ChangeState(startAwarness);
        prey = null;
        suspiciousFactor = 0f;
        ClearFollow();
    }
}
