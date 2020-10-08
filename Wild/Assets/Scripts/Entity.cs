using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    protected void FixedUpdate() {
        if (isDashing)
            UpdateDash();
        else
            UpdateMove();
    }

    #endregion

    #region Movements

    protected void UpdateMove() {
        if(MoveDirection != Vector3.zero) {
            
            // Turn friction
            float angle = Vector2.SignedAngle(velocity, MoveDirection);
            if (angle != 0)
            {
                float angleRatio = Mathf.Abs(angle) / 360f;
                float frictionToApply = turnFriction * angleRatio * Time.fixedDeltaTime;
                velocity += velocity.normalized * frictionToApply;
            }

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
            rigidbody.velocity = velocity;
        } else {
            Vector3 pos = Position;
            pos.x = velocity.x * Time.fixedDeltaTime;
            pos.y = velocity.y * Time.fixedDeltaTime;
            Position = pos;
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
            pos.x = dashDirection.x * dashSpeed * Time.fixedDeltaTime;
            pos.y = dashDirection.y * dashSpeed * Time.fixedDeltaTime;
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

    public void MoveTo(Vector3 dir, float speed) {
        Position = new Vector3(
            transform.position.x + dir.x * speed,
            transform.position.y + dir.y * speed,
            transform.position.y + dir.z * speed
        );
    }

    public void MoveToDestination(Vector3 dest) {
        transform.position = dest;
    }

    public void Follow(GameObject go) { }
}
