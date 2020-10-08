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
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 direction = Vector3.up;
    private Vector3 velocity = Vector3.zero;

    private GameObject goToFollow = null;
    private Vector3 followPositionDelta = Vector3.zero;
    private Vector3 destination = Vector3.zero;
    private bool goToDestination = false;

    public bool usePathFinding;

    public float refreshPathDuration = 2f;
    private float refreshPathCountdown = -1f;
    private int followPathIndex = 0;
    private NavMeshPath navPath;

    [Header("Dash")]
    public float dashTime = 1f;
    public float dashSpeed = 50f;
    private float dashCountdown = -1f;
    private bool isDashing = false;
    private Vector3 dashDirection = Vector3.zero;

    protected Rigidbody2D rigidbody = null;

    #region Properties

    public Vector3 Position {
        get { return transform.position; }
        set { transform.position = value; }
    }

    private Vector3 MoveDirection {
        get { return moveDirection; }
        set {
            moveDirection = value;
            if(moveDirection != Vector3.zero)
                direction = moveDirection;
        }
    }

    public bool IsDashing {
        get { return isDashing; }
        private set {}
    }

    #endregion

    #region Unity callbacks

    protected void Awake() {
        EntitiesManager.AddEntity(this);
    }

    protected void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
        navPath = new NavMeshPath();
    }

    protected void FixedUpdate() {
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

                if (navPath.corners.Length == 0) {
                    RefreshPath();
                }

                if((new Vector2(navPath.corners[followPathIndex].x, navPath.corners[followPathIndex].z) - new Vector2(Position.x, Position.z)).sqrMagnitude <= 1f) {
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

            } else { // Sans PathFinding
                if ((new Vector2(destination.x, destination.z) - new Vector2(Position.x, Position.z)).sqrMagnitude <= 2f) {
                    goToDestination = false;
                    MoveDir(Vector2.zero);
                } else {
                    MoveToward(destination);
                }
            }
        }

        if (MoveDirection != Vector3.zero) {
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
                velocity = Vector3.zero;
            }
        }
    }

    public void ApplySpeed() {
        if (rigidbody != null) {
            rigidbody.velocity = new Vector3(velocity.x, 0, velocity.y);
        } else {
            Vector3 pos = Position;
            pos.x = pos.x + velocity.x * Time.fixedDeltaTime;
            pos.z = pos.z + velocity.y * Time.fixedDeltaTime;
            Position = pos;
        }
    }

    private void RefreshPath()
    {
        Vector3 originPos = Position;

        Vector3 destinationPos = new Vector3(destination.x, Position.y, destination.z);

        bool pathFound = NavMesh.CalculatePath(originPos, destinationPos, NavMesh.AllAreas, navPath);
        if (pathFound) {
            followPathIndex = 1;
        } else {
            Debug.Log("Path Not Found");
        }
    }

    #endregion

    #region Dash

    public void Dash() {
        isDashing = true;
        dashCountdown = dashTime;
        dashDirection = direction;
    }

    public void UpdateDash() {
        if(dashCountdown > 0) {
            dashCountdown -= Time.fixedDeltaTime;
            Vector3 pos = Position;
            pos.x = pos.x + dashDirection.x * dashSpeed * Time.fixedDeltaTime;
            pos.z = pos.z + dashDirection.y * dashSpeed * Time.fixedDeltaTime;
            Position = pos;
        } else {
            isDashing = false;
            velocity = dashDirection * speedMax;
        }
    }

    #endregion

    public void MoveDir(Vector2 dir) {
        MoveDirection = dir.normalized;
    }

    public void MoveToward(Vector3 pos) {
        MoveToward(new Vector2(pos.x, pos.z));
    }

    public void MoveToward(Vector2 pos)
    {
        MoveDir(pos - new Vector2(Position.x, Position.z));
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
    }

    public void Follow(GameObject go) {
        goToFollow = go;
        followPositionDelta = Vector3.zero;
    }

    public void FollowLock(GameObject go)
    {
        goToFollow = go;
        followPositionDelta = Position - go.transform.position;
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
}
