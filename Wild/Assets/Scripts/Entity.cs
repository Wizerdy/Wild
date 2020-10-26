using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Entity : MonoBehaviour {
    [Header("Identity")]
    public string entityId;
    public string[] entityGroup;

    [Header("Movements")]
    public float acceleration = 5f;
    public float speedMax = 10f;
    public float friction = 20f;
    public float turnFriction = 20f;
    private Vector2 moveDirection = Vector3.zero;
    protected Vector2 direction = Vector3.up;
    protected Vector2 velocity = Vector2.zero;

    private GameObject goToFollow = null;
    private Vector3 followPositionDelta = Vector3.zero;
    private Vector3 destination = Vector3.zero;
    private bool goToDestination = false;

    public bool usePathFinding;

    public float refreshPathDuration = 2f;
    private float refreshPathCountdown = -1f;
    private int followPathIndex = 0;
    private NavMeshPath navPath;

    public float destinationRadius = 2f;

    public bool rotate = false;

    private Coroutine forcedMovementsRoutine = null;
    private bool forcedMovements = false;

    [Header("Dash")]
    public float dashTime = 1f;
    public float dashSpeed = 50f;
    public float dashCooldown = 5f;
    private bool canDash = true;
    private float dashCountdown = -1f;
    private bool isDashing = false;
    private Vector3 dashDirection = Vector3.zero;

    protected Rigidbody rigidbody = null;

    #region Properties

    public Vector3 Position {
        get { return transform.position; }
        set { transform.position = value; }
    }

    private Vector2 MoveDirection {
        get { return moveDirection; }
        set {
            moveDirection = value;
            if(moveDirection != Vector2.zero)
                direction = moveDirection;
        }
    }

    public bool IsDashing {
        get { return isDashing; }
        private set {}
    }

    public bool CanDash {
        get { return canDash; }
        private set {}
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
        rigidbody = GetComponent<Rigidbody>();
        navPath = new NavMeshPath();
    }

    protected virtual void FixedUpdate() {
        if (isDashing)
            UpdateDash();
        else
            UpdateMove();

        ApplySpeed();
    }

    #endregion

    #region Movements

    protected void UpdateMove() {
        if(goToFollow != null) {
            MoveToDestination(goToFollow.transform.position + followPositionDelta);
        }
        
        if (goToDestination) {
            if (usePathFinding) { // Avec Path finding
                if (navPath.corners == null || navPath.corners.Length == 0) {
                    RefreshPath();
                }

                //if(IsNearPoint(new Vector2(navPath.corners[followPathIndex].x, navPath.corners[followPathIndex].z), 1f)) {
                if(IsNearPoint(navPath.corners[followPathIndex].ConvertTo2D(), destinationRadius)) {
                    followPathIndex++;
                    if(followPathIndex >= navPath.corners.Length) {
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

        if (MoveDirection != Vector2.zero) {
            // Turn friction
            float angle = Vector2.SignedAngle(velocity, MoveDirection);
            float angleRatio = Mathf.Abs(angle) / 360f;
            float frictionToApply = turnFriction * angleRatio * Time.fixedDeltaTime;
            velocity -= velocity.normalized * frictionToApply;

            // Accélération
            velocity += MoveDirection * acceleration * Time.fixedDeltaTime;
            if (velocity.sqrMagnitude > speedMax * speedMax) {
                velocity = velocity.normalized * speedMax;
            }
        } else {
            // Décélération
            float frictionToApply = friction * Time.fixedDeltaTime;
            if(velocity.sqrMagnitude > frictionToApply * frictionToApply) {
                velocity -= velocity.normalized * frictionToApply;
            } else {
                velocity = Vector2.zero;
            }
        }
    }

    public void ApplySpeed() {
        if (rigidbody != null) {
            //rigidbody.velocity = new Vector3(velocity.x, 0, velocity.y);
            rigidbody.velocity = velocity.ConvertTo3D();
        } else {
            Vector3 pos = Position;
            pos.x = pos.x + velocity.x * Time.fixedDeltaTime;
            pos.z = pos.z + velocity.y * Time.fixedDeltaTime;
            Position = pos;
        }

        if (rotate) {
            LookAt(direction);
        }
    }

    private void RefreshPath()
    {
        Vector3 originPos = Position;

        //Vector3 destinationPos = new Vector3(destination.x, Position.y, destination.z);
        Vector3 destinationPos = destination.OverwriteY(Position.y);

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

    #endregion

    #region Movements controller

    public void MoveInstant(Vector3 pos) {
        transform.position = pos;
    }

    public void MoveDir(Vector2 dir) {
        MoveDirection = dir.normalized;
    }

    public void MoveToward(Vector3 pos) {
        //MoveToward(new Vector2(pos.x, pos.z));
        MoveToward(pos.ConvertTo2D());
    }

    public void MoveToward(Vector2 pos) {
        //MoveDir(pos - new Vector2(Position.x, Position.z));
        MoveDir(pos - Position.ConvertTo2D());
    }

    public void MoveTo(Vector3 dir, float speed) {
        Position = new Vector3(
            transform.position.x + dir.x * speed,
            transform.position.y + dir.y * speed,
            transform.position.y + dir.z * speed
        );
    }

    public void MoveToDestination(Vector3 dest) {
        destination = dest;
        goToDestination = true;
        if(usePathFinding) {
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

    public void FollowLock(GameObject go)
    {
        goToFollow = go;
        followPositionDelta = Position - go.transform.position;
        if (usePathFinding) {
            RefreshPath();
        }
    }

    public void ClearFollow()
    {
        goToFollow = null;
        followPositionDelta = Vector3.zero;
    }

    public void CopyMovementValues(Entity entity)
    {
        acceleration = entity.acceleration;
        speedMax = entity.speedMax;
        friction = entity.friction;
        turnFriction = entity.turnFriction;
    }

    public void ResetVelocityX()
    {
        velocity.x = 0f;
    }

    public void ResetVelocityY()
    {
        velocity.y = 0f;
    }

    #endregion

    #region Dash

    public void Dash() {
        isDashing = true;
        dashCountdown = dashTime;
        dashDirection = direction;
    }

    public void UpdateDash() {
        if (dashCountdown > 0) {
            dashCountdown -= Time.fixedDeltaTime;
            if (rigidbody != null) {
                velocity = dashDirection * dashSpeed;
            }
        } else {
            isDashing = false;
            velocity = dashDirection * speedMax;
            if (rigidbody != null) {
                //rigidbody.velocity = new Vector3(velocity.x, 0, velocity.y);
                rigidbody.velocity = velocity.ConvertTo3D();
            }
            StartCoroutine(DashCooldown(dashCooldown));
        }
    }

    #endregion

    protected bool IsNearPoint(Vector2 pos, float radius) {
        //if((pos - new Vector2(Position.x, Position.z)).sqrMagnitude <= radius * radius) {
        if((pos - Position.ConvertTo2D()).sqrMagnitude <= radius * radius) {
            return true;
        }
        return false;
    }

    protected void ChangeAlphaMaterial(GameObject obj, byte alpha) {
        if (obj.GetComponent<Renderer>() != null) {
            Renderer renderer = obj.GetComponent<Renderer>();
            Color32 col = renderer.material.GetColor("_BaseColor");
            col.a = alpha;
            renderer.material.SetColor("_BaseColor", col);
        }
    }

    IEnumerator DashCooldown(float time) {
        canDash = false;
        yield return new WaitForSeconds(time);
        canDash = true;
    }

    public void DoMoveLerp(Vector3 destination, float time, int steps)
    {
        if (forcedMovementsRoutine != null) { StopCoroutine(forcedMovementsRoutine); }

        forcedMovementsRoutine = StartCoroutine(MoveLerp(destination, time, steps));
        forcedMovements = true;
    }

    IEnumerator MoveLerp(Vector3 destination, float time, int steps)
    {
        Vector3 pos = Position;
        for (int i = 0; i < steps; i++)
        {
            yield return new WaitForSeconds(time / (float)steps);
            MoveInstant(Vector3.Lerp(pos, destination, (float)i / (float)steps));
        }
        MoveInstant(destination);
        forcedMovements = false;
    }

    ~Entity() {
        Debug.Log("Yay !" + gameObject.name);
        EntitiesManager.RemoveEntity(this);
    }
}
