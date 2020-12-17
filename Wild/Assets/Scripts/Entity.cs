using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using SoundManager;

[SelectionBase]
public class Entity : MonoBehaviour {
    private const int MOVE_FPS = 60;

    [Serializable]
    public class MovementCurve {
        public AnimationCurve curve;
        public float duration;
        [HideInInspector] public float timer;

        #region Constructeurs

        public MovementCurve(AnimationCurve curve, float duration, float timer) {
            this.curve = curve;
            this.duration = duration;
            this.timer = timer;
        }

        public MovementCurve(AnimationCurve curve) : this(curve, 1f, 0f) { }

        public MovementCurve() : this(AnimationCurve.Linear(0f, 0f, 1f, 1f)) { }

        #endregion

        public float GetRatio() {
            float ratio = timer / duration;
            ratio = curve.Evaluate(ratio);
            return ratio;
        }

        public MovementCurve Clone() {
            return new MovementCurve(curve, duration, timer);
        }
    }

    [Header("Identity")]
    public string entityId;
    public string[] entityGroup;

    [Header("Movements")]
    public float speedMax = 10f;
    [HideInInspector] public float defaultSpeedMax;
    [HideInInspector] public float speedMaxGlobal;

    private Vector2 moveDirection = Vector2.zero;
    private Vector2 prevMoveDirection = Vector2.zero;
    protected Vector2 orientDir = Vector2.up;
    protected Vector2 direction = Vector2.up;
    private bool wasMoving = false;

    protected Vector2 velocity = Vector2.zero;
    [Space]

    [SerializeField] public MovementCurve speed = new MovementCurve();
    [SerializeField] protected MovementCurve acceleration = new MovementCurve();
    [SerializeField] protected MovementCurve frictions = new MovementCurve();
    [SerializeField] protected MovementCurve turnAround = new MovementCurve();
    private float turnAroundVelocityStart = 0f;
    private bool isTurningAround = false;

    [Space]
    [SerializeField] protected MovementCurve turn = new MovementCurve();

    [SerializeField, Range(0f, 90f)] protected float turnAngleMin = 1f;
    [SerializeField, Range(0f, 90f)] protected float turnAngleMax = 180f;
    [SerializeField] protected float turnDurationMin = 0.1f;
    [SerializeField] protected float turnDurationMax = 1f;
    private Vector3 turnVelocityStart = Vector3.zero;
    private bool isTurning = false;

    [Space]
    // Old
    //public float acceleration = 5f;
    //public float friction = 20f;
    //public float turnFriction = 20f;
    public bool usePathFinding;

    public bool underEffect;
    private GameObject goToFollow = null;
    private Vector3 followPositionDelta = Vector3.zero;
    private Vector3 destination = Vector3.zero;
    protected bool goToDestination = false;
    protected Coroutine startDashCooldown;

    public float refreshPathDuration = 2f;
    private float refreshPathCountdown = -1f;
    private int followPathIndex = 0;
    private NavMeshPath navPath;

    public float destinationRadius = 2f;

    public bool rotateTowardDirection = false;

    private Coroutine forcedMovementsRoutine = null;
    private bool forcedMovements = false;

    private Vector3 rotateDestination = Vector3.zero;
    private bool goToRotation = false;
    public AnimationCurve rotationAcceleration;

    protected Transform rotatingTransform = null;

    [Header("Dash")]
    [SerializeField] protected MovementCurve dash = new MovementCurve();
    public float dashSpeed = 50f;
    public float dashCooldown = 5f;
    private bool canDash = true;
    private bool isDashing = false;
    private Vector3 dashDirection = Vector3.zero;
    private float dashVelocityStart = 0f;

    protected Rigidbody rigidBody = null;

    private List<AreaTrigger> areaTriggers = new List<AreaTrigger>();

    [Header("Sound")]
    [SerializeField] protected SoundObject stepSound = null;

    #region Properties

    public Vector3 Position {
        get { return transform.position; }
        set { transform.position = value; }
    }

    protected Vector2 MoveDirection {
        get { return moveDirection; }
        set {
            moveDirection = value;
            if (moveDirection != Vector2.zero)
                direction = moveDirection;
        }
    }

    public Vector2 OrientDir
    {
        get { return orientDir; }
    }

    #region Movements properties

    public MovementCurve Speed {
        get { return acceleration.Clone(); }
        private set { }
    }

    public MovementCurve Acceleration {
        get { return acceleration.Clone(); }
        private set { }
    }

    public MovementCurve Frictions {
        get { return acceleration.Clone(); }
        private set { }
    }

    public MovementCurve TurnAround {
        get { return acceleration.Clone(); }
        private set { }
    }

    public MovementCurve Turn {
        get { return acceleration.Clone(); }
        private set { }
    }

    #endregion

    public bool IsDashing {
        get { return isDashing; }
        private set { }
    }

    public bool CanDash {
        get { return canDash; }
        set { }
    }

    public bool IsMovementForced {
        get { return forcedMovements; }
        private set { }
    }

    #endregion

    #region Unity callbacks

    protected virtual void Awake() {
        EntitiesManager.AddEntity(this);
    }

    protected virtual void Start() {
        rigidBody = GetComponent<Rigidbody>();
        navPath = new NavMeshPath();
        defaultSpeedMax = speedMax;
        speedMaxGlobal = speedMax;

        rotatingTransform = transform.Find("Rotate");
    }

    protected virtual void FixedUpdate() {
        if (isDashing) {
            UpdateDash();
        } else if (!IsMovementForced) {
            UpdateMove();
            UpdateRotation();
        }
        if (!IsMovementForced)
            ApplySpeed();

        if (rotatingTransform != null) {
            rotatingTransform.localEulerAngles = Vector3.zero.Overwrite(Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg, Tools.IgnoreMode.Y);
        }
    }

    #endregion

    #region Movements

    protected void UpdateMove() {
        if (goToFollow != null) {
            MoveToDestination(goToFollow.transform.position + followPositionDelta);
        }

        if (goToDestination) {
            UpdateDestination();
        }

        bool isMoving = (moveDirection != Vector2.zero);

        if (isMoving) {
            if (velocity != Vector2.zero) {
                if (Vector2.Dot(prevMoveDirection, moveDirection) < 0f) {
                    StartTurnAround();
                } else {
                    float angle = Vector2.Angle(prevMoveDirection, moveDirection);
                    if (angle > turnAngleMin) {
                        StartTurn(angle);
                    }
                }
            } else {
                StartAcceleration();
            }

            if (isTurningAround) {
                velocity = ApplyTurnAround(velocity);
            } else {
                Vector2 velocity = ApplyAcceleration();
                this.velocity = ApplyTurn(velocity);
                direction = this.velocity.normalized;
            }

            if (velocity != Vector2.zero) {
                orientDir = velocity.normalized;
            }

            prevMoveDirection = moveDirection;
        } else {
            if (wasMoving) {
                StartFrictions();
            }

            isTurning = false;
            isTurningAround = false;

            velocity = ApplyFrictions(velocity);
        }

        wasMoving = isMoving;

        // Old
        //if (MoveDirection != Vector2.zero) {
        //    // Turn friction
        //    float angle = Vector2.SignedAngle(velocity, MoveDirection);
        //    float angleRatio = Mathf.Abs(angle) / 360f;
        //    float frictionToApply = turnFriction * angleRatio * Time.fixedDeltaTime;
        //    velocity -= velocity.normalized * frictionToApply;

        //    // Accélération
        //    velocity += MoveDirection * acceleration * Time.fixedDeltaTime;
        //    if (velocity.sqrMagnitude > speedMax * speedMax) {
        //        velocity = velocity.normalized * speedMax;
        //    }
        //} else {
        //    // Décélération
        //    float frictionToApply = friction * Time.fixedDeltaTime;
        //    if(velocity.sqrMagnitude > frictionToApply * frictionToApply) {
        //        velocity -= velocity.normalized * frictionToApply;
        //    } else {
        //        velocity = Vector2.zero;
        //    }
        //}
    }

    private void StartAcceleration() {
        float currentSpeed = velocity.magnitude;
        float accelerationTimerRatio = currentSpeed / speedMax;
        acceleration.timer = Mathf.Lerp(0f, acceleration.duration, accelerationTimerRatio);
        isTurning = false;
    }

    private Vector2 ApplyAcceleration() {
        acceleration.timer += Time.deltaTime;
        if (acceleration.timer < acceleration.duration) {
            float ratio = acceleration.GetRatio();
            float speed = Mathf.Lerp(0f, speedMax, ratio);

            return moveDirection * speed;
        } else {
            return moveDirection * speedMax;
        }
    }

    private void StartFrictions() {
        float currentSpeed = velocity.magnitude;
        float frictionTimerRatio = Mathf.InverseLerp(0f, speedMax, currentSpeed);
        frictions.timer = Mathf.Lerp(frictions.duration, 0f, frictionTimerRatio);
    }

    private Vector2 ApplyFrictions(Vector2 velocity) {
        frictions.timer += Time.deltaTime;
        if (frictions.timer < frictions.duration) {
            float ratio = frictions.GetRatio();
            float speed = Mathf.Lerp(speedMax, 0f, ratio);
            velocity = velocity.normalized * speed;
        } else {
            velocity = Vector2.zero;
            prevMoveDirection = Vector2.zero;
        }

        return velocity;
    }

    private void StartTurnAround() {
        isTurningAround = true;
        turnAround.timer = 0f;
        isTurning = false;
        turnAroundVelocityStart = velocity.magnitude;
    }

    private Vector2 ApplyTurnAround(Vector2 velocity) {
        turnAround.timer += Time.deltaTime;
        if (turnAround.timer < turnAround.duration) {
            float ratio = turnAround.GetRatio();
            float speed = Mathf.Lerp(turnAroundVelocityStart, 0f, ratio);
            velocity = velocity.normalized * speed;
        } else {
            velocity = Vector2.zero;
            acceleration.timer = 0f;
            isTurningAround = false;
        }

        return velocity;
    }

    private void StartTurn(float angle) {
        float turnDurationRatio = Mathf.InverseLerp(turnAngleMin, turnAngleMax, angle);
        turn.duration = Mathf.Lerp(turnDurationMin, turnDurationMax, turnDurationRatio);
        turn.timer = 0f;
        turnVelocityStart = velocity;
        isTurning = true;
    }

    private Vector2 ApplyTurn(Vector2 velocity) {
        if (isTurning) {
            turn.timer += Time.deltaTime;
            if (turn.timer < turn.duration) {
                float ratio = turn.GetRatio();
                velocity = Vector2.Lerp(turnVelocityStart, velocity, ratio);
            } else {
                isTurning = false;
            }
        }

        return velocity;
    }

    private void UpdateDestination() {
        if (usePathFinding) { // Avec Path finding
            if (navPath.corners == null || navPath.corners.Length == 0) {
                RefreshPath();
            }

            //if(IsNearPoint(new Vector2(navPath.corners[followPathIndex].x, navPath.corners[followPathIndex].z), 1f)) {
            if (IsNearPoint(navPath.corners[followPathIndex].ConvertTo2D(), destinationRadius)) {
                followPathIndex++;
                if (followPathIndex >= navPath.corners.Length) {
                    goToDestination = false;
                    followPathIndex = 0;
                    MoveDir(Vector2.zero);
                    return;
                }
            } else {
                refreshPathCountdown -= Time.deltaTime;
                if (refreshPathCountdown <= 0f) {
                    refreshPathCountdown = refreshPathDuration;
                    RefreshPath();
                }
                MoveToward(navPath.corners[followPathIndex]);
            }

            for (int i = 0; i < navPath.corners.Length - 1; i++) {
                Debug.DrawLine(navPath.corners[i], navPath.corners[i + 1], Color.blue);
            }

        } else { // Sans PathFinding
                 //if (IsNearPoint(new Vector2(destination.x, destination.z), 2f)) {
            if (IsNearPoint(destination.ConvertTo2D(), destinationRadius)) {
                goToDestination = false;
                MoveDir(Vector2.zero);
            } else {
                MoveToward(destination);
            }
        }
    }

    private void UpdateRotation() {
        if (goToRotation) {
            transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, rotateDestination, 1f); // TO CHANGE
        } else if (rotateTowardDirection) {
            if (direction != Vector2.zero) { LookAt(direction); }
        }
    }

    public void ApplySpeed() {
        if (rigidBody != null) {
            //rigidbody.velocity = new Vector3(velocity.x, 0, velocity.y);
            rigidBody.velocity = velocity.ConvertTo3D();
        } else {
            Vector3 pos = Position;
            pos.x = pos.x + velocity.x * Time.fixedDeltaTime;
            pos.z = pos.z + velocity.y * Time.fixedDeltaTime;
            Position = pos;
        }
    }

    private void RefreshPath() {
        Vector3 originPos = Position;

        //Vector3 destinationPos = new Vector3(destination.x, Position.y, destination.z);
        Vector3 destinationPos = destination.Overwrite(Position.y, Tools.IgnoreMode.Y);

        bool pathFound = NavMesh.CalculatePath(originPos, destinationPos, NavMesh.AllAreas, navPath);
        if (pathFound) {
            followPathIndex = 1;
        } else {
            Debug.Log("Path Not Found");
        }
    }

    private void LookAt(Vector2 dest) {
        transform.LookAt(Position + dest.ConvertTo3D(Position.y));
    }

    //private void LookAt(Vector2 dir) {
    //    float angle = Vector2.SignedAngle(Vector2.right, dir);
    //    Vector3 eulerAngles = transform.localEulerAngles;
    //    eulerAngles.y = angle;
    //    transform.localEulerAngles = eulerAngles;
    //}

    #endregion

    #region Movements controller

    public void MoveInstant(Vector3 pos) {
        transform.position = pos;
    }

    public void MoveDir(Vector2 dir) {
        MoveDirection = dir.normalized;
    }

    public void MoveToward(Vector3 pos) {
        MoveToward(pos.ConvertTo2D());
    }

    public void MoveToward(Vector2 pos) {
        MoveDir(pos - Position.ConvertTo2D());
    }

    public void MoveToDestination(Vector3 dest) {
        destination = dest;
        goToDestination = true;
        if (usePathFinding) {
            RefreshPath();
        }
    }

    public void Follow(GameObject go) {
        goToFollow = go;
        followPositionDelta = Vector3.zero;
        if (usePathFinding) {
            RefreshPath();
        }
    }

    public void FollowLock(GameObject go) {
        goToFollow = go;
        followPositionDelta = Position - go.transform.position;
        if (usePathFinding) {
            RefreshPath();
        }
    }

    public void DoMoveLerp(Vector3 destination, float time) {
        if (forcedMovementsRoutine != null) { StopCoroutine(forcedMovementsRoutine); }

        forcedMovementsRoutine = StartCoroutine(MoveLerp(destination, time));
        forcedMovements = true;
    }

    public void StopMove(float time)
    {
        DoMoveLerp(transform.position, time);
    }

    public void ClearFollow() {
        goToFollow = null;
        followPositionDelta = Vector3.zero;
    }

    public void CopyMovementValues(Entity entity) {
        speedMax = entity.speedMax;
        speed = entity.Speed;
        acceleration = entity.Acceleration;
        frictions = entity.Frictions;
        turn = entity.Turn;
        turnAround = entity.TurnAround;
    }

    public void ResetVelocityX() {
        velocity.x = 0f;
    }

    public void ResetVelocityY() {
        velocity.y = 0f;
    }

    #endregion

    #region Dash

    public void Dash() {
        if(canDash == false) { return; }

        canDash = false;
        isDashing = true;
        dashDirection = direction;

        dashVelocityStart = velocity.magnitude;
        float dashTimerRatio = Mathf.InverseLerp(0f, dashSpeed, dashVelocityStart);
        dash.timer = Mathf.Lerp(0f, dash.duration, dashTimerRatio);
    }

    public void UpdateDash() {
        if (dash.timer < dash.duration) {
            dash.timer += Time.fixedDeltaTime;
            float ratio = dash.GetRatio();
            float speed = Mathf.Lerp(dashVelocityStart, dashSpeed, ratio);
            velocity = dashDirection * speed;
            if (rigidBody != null) {
                rigidBody.velocity = velocity;
            }
        } else {
            isDashing = false;
            velocity = dashDirection * speedMax;
            if (rigidBody != null) {
                rigidBody.velocity = velocity.ConvertTo3D();
            }
            StartDashCooldown(dashCooldown);
            //StartCoroutine(DashCooldown(dashCooldown));
        }
    }

    #endregion

    #region AreaTriggers

    public void EnterAreaTrigger(AreaTrigger trigger) {
        if (areaTriggers.Contains(trigger)) { areaTriggers.Add(trigger); return; }
        areaTriggers.Add(trigger);

        trigger.OnAreaEnter(this);
    }

    public void ExitAreaTrigger(AreaTrigger trigger) {
        if (!areaTriggers.Contains(trigger)) { return; }

        if (areaTriggers.Count > 1) {
            if (trigger.alwaysExitActions) {
                trigger.OnAreaExit(this);
            }
            areaTriggers.Remove(trigger);

            if (trigger != areaTriggers[areaTriggers.Count - 1]) {
                areaTriggers[areaTriggers.Count - 1].OnAreaEnter(this);
            }
        } else {
            trigger.OnAreaExit(this);
            areaTriggers.Remove(trigger);
        }
    }

    #endregion

    protected bool IsNearPoint(Vector2 pos, float radius) {
        //if((pos - new Vector2(Position.x, Position.z)).sqrMagnitude <= radius * radius) {
        if ((pos - Position.ConvertTo2D()).sqrMagnitude <= radius * radius) {
            return true;
        }
        return false;
    }

    IEnumerator DashCooldown(float time)
    {
        canDash = false;
        GameManager.Instance.dashButton.GetComponent<Image>().color = GameManager.Instance.dashColor[0];
        yield return new WaitForSeconds(time);
        canDash = true;
        GameManager.Instance.dashButton.GetComponent<Image>().color = GameManager.Instance.dashColor[1];
    }

    public void StartDashCooldown(float time)
    {
        if (null != startDashCooldown)
        {
            StopCoroutine(startDashCooldown);
        }
        startDashCooldown = StartCoroutine(DashCooldown(time));
    }

    public IEnumerator SpeedReducedForSeconds(float speed, float time)
    {
        float tempSpeed = speedMaxGlobal;
        speedMaxGlobal = speed;
        yield return new WaitForSeconds(time);
        underEffect = false;
        speedMaxGlobal = tempSpeed;
    }

    public void StartSpeedReducedForSeconds(float speed, float time)
    {
        StartCoroutine(SpeedReducedForSeconds(speed, time));
    }

    public void SetSpeed(float runSpeed, float walkSpeed)
    {
        runSpeed = speedMax;
        walkSpeed = speedMax / 2;
    }

    public bool IsEntityId(string id)
    {
        if (entityId.Equals(id))
        {
            return true;
        }
        return false;
    }

    IEnumerator MoveLerp(Vector3 destination, float time) {
        Vector3 pos = Position;
        float frames = time * (float)MOVE_FPS;
        for (int i = 0; i < frames; i++) {
            yield return new WaitForSeconds(time / (float)frames);
            MoveInstant(Vector3.Lerp(pos, destination, (float)i / (float)frames));
        }
        MoveInstant(destination);
        forcedMovements = false;
    }

    public void PlayStepSound() {
        stepSound.Play();
    }

    ~Entity() {
        Debug.Log("Yay !" + gameObject.name);
        EntitiesManager.RemoveEntity(this);
    }
}
