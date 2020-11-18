using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JeromeEntity : MonoBehaviour
{
    #region Properties

    //Movement
    private Vector2 _moveDir = Vector2.zero;
    private Vector2 _previousMoveDir = Vector2.zero;
    private bool _wasMoving = false;

    //Speed
    [Header("Speed")]
    [SerializeField] private float _speedMax = 5f;
    private Vector3 _velocity = Vector3.zero;

    //Acceleration 
    [Header("Acceleration")]
    [SerializeField] private float _accelerationDuration = 1f;
    [SerializeField] private AnimationCurve _accelerationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    private float _accelerationTimer = 0f;

    //Frictions
    [Header("Frictions")]
    [SerializeField] private float _frictionsDuration = 1f;
    [SerializeField] private AnimationCurve _frictonsCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    private float _frictionsTimer = 0f;

    //Turn
    [Header("Turn")]
    [SerializeField, Range(0f, 90f)] private float _turnAngleMin = 1f;
    [SerializeField, Range(0f, 90f)] private float _turnAngleMax = 180f;
    [SerializeField] private float _turnDurationMin = 0.1f;
    [SerializeField] private float _turnDurationMax = 1f;
    [SerializeField] private AnimationCurve _turnCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    private float _turnTimer = 0f;
    private float _turnDuration = 0f;
    private Vector3 _turnVelocityStart = Vector3.zero;
    private bool _isTurning = false;

    //Turn Around
    [Header("Turn Around")]
    [SerializeField] private float _turnAroundDuration = 1f;
    [SerializeField] private AnimationCurve _turnAroundCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    private float _turnAroundTimer = 0f;
    private bool _isTurningAround = false;

    //Dash
    [Header("Dash")]
    [SerializeField] private float _dashSpeedMin = 4f;
    [SerializeField] private float _dashSpeedMax = 50f;
    [SerializeField] private float _dashDuration = 0.1f;
    [SerializeField] private AnimationCurve _dashCurve = AnimationCurve.Constant(0f, 1f, 1f);
    private float _dashTimer = 0f;
    private bool _isDashing = false;
    private Vector2 _dashDir = Vector2.zero;

    //Orient
    [Header("Orient")]
    [SerializeField] private GameObject _orientModel = null;
    private Vector2 _orientDir = Vector2.right;

    //Debug
    [Header("Debug")]
    [SerializeField] private bool _guiDebug = true;

    private Rigidbody2D _rigidbody = null;

    #endregion

    #region Functions Unity Callbacks

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (_isDashing)
        {
            _UpdateDash();
        }
        else
        {
            _UpdateMove();
        }
        _ApplyVelocity();
        _UpdateModelOrient();
    }

    private void OnGUI()
    {
        if (!_guiDebug) return;

        GUIStyle guiStyle = new GUIStyle();
        guiStyle.normal.textColor = Color.white;
        guiStyle.fontSize = 40;

        GUILayout.Label("MoveDir = " + _moveDir, guiStyle);
        GUILayout.Label("PrevMoveDir = " + _previousMoveDir, guiStyle);
        GUILayout.Label("Velocity = " + _velocity, guiStyle);
        if (_isTurning)
        {
            GUILayout.Label("TurnTimer = " + _turnTimer, guiStyle);
            GUILayout.Label("TurnDuration = " + _turnDuration, guiStyle);
        }
        if (_isTurningAround)
        {
            GUILayout.Label("TurnAroundTimer = " + _turnAroundTimer, guiStyle);
        }
        if (_accelerationTimer < _accelerationDuration)
        {
            GUILayout.Label("Acceleration Timer = " + _accelerationTimer, guiStyle);
        }
        if (_frictionsTimer < _frictionsDuration)
        {
            GUILayout.Label("Frictions Timer = " + _accelerationTimer, guiStyle);
        }
    }

    #endregion

    #region Functions Move

    public void Move(Vector2 moveDir)
    {
        _moveDir = moveDir;
    }

    private void _UpdateMove()
    {
        bool isMoving = _moveDir != Vector2.zero;

        if (isMoving)
        {
            //Here we check if velocity is set
            //If entity already has velocity, we need to check if we need to turn
            if (_velocity != Vector3.zero)
            {
                //Check if previous move dir is opposite to current move dir
                //If move dirs are opposite, we force entity to turn around
                if (Vector3.Dot(_previousMoveDir, _moveDir) < 0f)
                {
                    _StartTurnAround();
                }
                else
                {
                    //Otherwise we calculate angle between the two directions and we force entity to turn with some delay
                    float angle = Vector3.Angle(_previousMoveDir, _moveDir);
                    if (angle > _turnAngleMin)
                    {
                        _StartTurn(angle);
                    }
                }
            }
            else
            {
                //Otherwise we start the acceleration timer
                _StartAcceleration();
            }

            if (_isTurningAround)
            {
                _velocity = _ApplyTurnAround(_velocity);
            }
            else
            {
                Vector3 velocity = _ApplyAcceleration();
                _velocity = _ApplyTurn(velocity);
                //Update Orient dir only when entity is moving
                _orientDir = _velocity.normalized;
            }

            //Store previous move dir to check when entity changes direction
            _previousMoveDir = _moveDir;
        }
        else
        {
            //Start friction if entity was moving on the previous frame
            if (_wasMoving)
            {
                _StartFrictions();
            }

            //Reset turns states
            _isTurning = false;
            _isTurningAround = false;

            //Apply frictions
            _velocity = _ApplyFrictions(_velocity);
        }

        _wasMoving = isMoving;
    }

    private void _StartAcceleration()
    {
        float currentSpeed = _velocity.magnitude;
        float accelerationTimerRatio = currentSpeed / _speedMax;
        _accelerationTimer = Mathf.Lerp(0f, _accelerationDuration, accelerationTimerRatio);
        _isTurning = false;
    }

    private Vector3 _ApplyAcceleration()
    {
        Vector3 velocity = Vector3.zero;
        _accelerationTimer += Time.deltaTime;
        if (_accelerationTimer < _accelerationDuration)
        {
            //Calculate acceleration according to timer and curve
            float ratio = _accelerationTimer / _accelerationDuration;
            ratio = _accelerationCurve.Evaluate(ratio);
            float speed = Mathf.Lerp(0f, _speedMax, ratio);
            return _moveDir * speed;
        }
        else
        {
            //Speed Max
            return _moveDir * _speedMax;
        }
    }

    private void _StartFrictions()
    {
        float currentSpeed = _velocity.magnitude;
        //Solution 1 : Inverse Lerp
        float frictionTimerRatio = Mathf.InverseLerp(0f, _speedMax, currentSpeed);
        //Solution 2 : Calculate ratio manually
        //float frictionTimer = _speedMax / currentSpeed;
        _frictionsTimer = Mathf.Lerp(_frictionsDuration, 0f, frictionTimerRatio);
    }

    private Vector3 _ApplyFrictions(Vector3 velocity)
    {
        _frictionsTimer += Time.deltaTime;
        if (_frictionsTimer < _frictionsDuration)
        {
            //Calculate Frictions according to timer and curve
            float ratio = _frictionsTimer / _frictionsDuration;
            ratio = _frictonsCurve.Evaluate(ratio);
            float speed = Mathf.Lerp(_speedMax, 0f, ratio);
            velocity = velocity.normalized * speed;
        }
        else
        {
            //Reset speed
            velocity = Vector2.zero;
            _previousMoveDir = Vector2.zero; ;
        }

        return velocity;
    }

    private void _StartTurnAround()
    {
        _isTurningAround = true;
        _turnAroundTimer = 0f;
        _isTurning = false;
    }

    private Vector3 _ApplyTurnAround(Vector3 velocity)
    {
        _turnAroundTimer += Time.deltaTime;
        if (_turnAroundTimer < _turnAroundDuration)
        {
            //Calculate TurnAround according to timer and curve
            float ratio = _turnAroundTimer / _turnAroundDuration;
            ratio = _turnAroundCurve.Evaluate(ratio);
            float speed = Mathf.Lerp(_speedMax, 0f, ratio);
            velocity = _velocity.normalized * speed;
        }
        else
        {
            velocity = Vector3.zero;
            _accelerationTimer = 0f;
            _isTurningAround = false;
        }

        return velocity;
    }

    private void _StartTurn(float angle)
    {
        float turnDurationRatio = Mathf.InverseLerp(_turnAngleMin, _turnAngleMax, angle);
        _turnDuration = Mathf.Lerp(_turnDurationMin, _turnDurationMax, turnDurationRatio);
        _turnTimer = 0f;
        _turnVelocityStart = _velocity;
        _isTurning = true;
    }

    private Vector3 _ApplyTurn(Vector3 velocity)
    {
        if (_isTurning)
        {
            //Turn Entity until timer is finished
            _turnTimer += Time.deltaTime;
            if (_turnTimer < _turnDuration)
            {
                float turnRatio = _turnTimer / _turnDuration;
                turnRatio = _turnCurve.Evaluate(turnRatio);
                velocity = Vector3.Lerp(_turnVelocityStart, velocity, turnRatio);
            }
            else
            {
                _isTurning = false;
            }
        }

        return velocity;
    }

    #endregion

    #region Functions Dash

    public void Dash()
    {
        _dashDir = _orientDir;
        _dashTimer = 0f;
        _isDashing = true;
    }

    private void _UpdateDash()
    {
        if (!_isDashing) return;

        _dashTimer += Time.deltaTime;
        if (_dashTimer < _dashDuration)
        {
            float dashRatio = 1f - (_dashTimer / _dashDuration);
            float dashSpeed = Mathf.Lerp(_dashSpeedMin, _dashSpeedMax, dashRatio);
            _velocity = _dashDir * dashSpeed;
        }
        else
        {
            float currentSpeed = _velocity.magnitude;
            float accelerationTimerRatio = currentSpeed / _speedMax;
            _accelerationTimer = Mathf.Lerp(0f, _accelerationDuration, accelerationTimerRatio);
            _isDashing = false;
        }
    }

    #endregion

    #region Functions Orient

    private void _UpdateModelOrient()
    {
        float angle = Vector2.SignedAngle(Vector2.right, _orientDir);
        Vector3 eulerAngles = _orientModel.transform.localEulerAngles;
        eulerAngles.z = angle;
        _orientModel.transform.localEulerAngles = eulerAngles;
    }

    #endregion

    #region Functions Velocity

    private void _ApplyVelocity()
    {
        if (null != _rigidbody)
        {
            _rigidbody.velocity = _velocity;
        }
        else
        {
            transform.position += _velocity * Time.fixedDeltaTime;
        }
    }

    #endregion
}