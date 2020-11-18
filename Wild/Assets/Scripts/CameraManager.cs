using UnityEngine;

[System.Serializable]
public class CameraTransition {
    public float duration = 0f;
    public AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
}

public class CameraManager : MonoBehaviour {
    public static CameraManager Instance { get; private set; }

    public enum PROFILE_MODE {
        DEFAULT = 0,
        OVERRIDE,
    }

    private Camera _camera;

    private float _defaultSize = 0f;
    private float _currentSize = 0f;

    private Vector3 _defaultPosition = Vector3.zero;
    private Vector3 _defaultRotation = Vector3.zero;

    private Transform _defaultParent = null;
    private Transform _currentParent = null;

    private CameraTransition _sizeTransition = null;
    private float _sizeTransitionStart = 0f;
    private float _sizeTransitionEnd = 0f;
    private float _sizeTransitionTimer = 0f;
    private bool _isSizeTransitionActive = false;

    private CameraTransition _moveTransition = null;
    private Vector3 _moveTransitionOrigin = Vector3.zero;
    private Vector3 _moveTransitionDestination = Vector3.zero;
    private float _moveTransitionTimer = 0f;

    private CameraTransition _rotateTransition = null;
    private Vector3 _rotateTransitionOrigin = Vector3.zero;
    private Vector3 _rotateTransitionDestination = Vector3.zero;
    private float _rotateTransitionTimer = 0f;

    private bool _isMoveTransitonActive = false;
    private bool _isRotateTransitonActive = false;

    private void Awake() {
        Instance = this;

        _camera = GetComponent<Camera>();
        _defaultSize = _currentSize = _camera.orthographicSize;

        _currentParent = transform.parent;
        _defaultParent = transform.parent;
    }

    private void Update() {
        _UpdateSizeTransition();
        _UpdateMoveTransition();
        //_UpdateZoomTransition();
        _UpdateRotateTransition();
    }

    public float DefaultSize => _defaultSize;
    public bool IsMoving => _isMoveTransitonActive;

    public void ApplyCameraProfile(Camera camera, PROFILE_MODE profileMode, CameraTransition transition = null) {
        _currentParent = camera.transform;
        if (profileMode == PROFILE_MODE.DEFAULT) {
            _defaultParent = camera.transform;
        }
        transform.SetParent(_currentParent);

        ChangeSize(camera.orthographicSize, profileMode, transition);
        MoveTo(_currentParent.transform.position, profileMode, transition);
        RotateTo(_currentParent.transform.position, profileMode, transition);
    }

    public void RestoreDefaultProfile(CameraTransition transition = null) {
        ChangeSize(_defaultSize, PROFILE_MODE.OVERRIDE, transition);

        _currentParent = _defaultParent;
        transform.SetParent(_currentParent);

        MoveTo(_currentParent.transform.position, PROFILE_MODE.OVERRIDE, transition);
        RotateTo(_defaultRotation, PROFILE_MODE.OVERRIDE, transition);
    }

    public void ChangeSize(float size, PROFILE_MODE profileMode, CameraTransition transition = null) {
        if (null != transition) {
            _sizeTransition = transition;
            _sizeTransitionStart = _currentSize;
            _sizeTransitionEnd = size;
            _sizeTransitionTimer = 0f;
            _isSizeTransitionActive = true;
        } else {
            _camera.orthographicSize = _currentSize = size;
        }

        if (profileMode == PROFILE_MODE.DEFAULT) {
            _defaultSize = size;
        }
    }

    public void MoveTo(Vector3 destination, PROFILE_MODE profileMode, CameraTransition transition = null) {
        if (null != transition) {
            _moveTransition = transition;
            _moveTransitionOrigin = transform.localPosition;
            //_moveTransitionDestination = destination;
            _moveTransitionDestination = _currentParent.InverseTransformPoint(destination);
            _moveTransitionTimer = 0f;
            _isMoveTransitonActive = true;
        } else {
            transform.position = destination;
        }

        if (profileMode == PROFILE_MODE.DEFAULT) {
            _defaultPosition = destination;
        }
    }

    public void RotateTo(Vector3 angle, PROFILE_MODE profileMode, CameraTransition transition = null) {
        if (null != transition) {
            _rotateTransition = transition;
            _rotateTransitionOrigin = transform.localEulerAngles;
            //_moveTransitionDestination = destination;
            _rotateTransitionDestination = angle;
            _rotateTransitionTimer = 0f;
            _isMoveTransitonActive = true;
        } else {
            transform.localEulerAngles = angle;
        }

        if (profileMode == PROFILE_MODE.DEFAULT) {
            _defaultRotation = angle;
        }
    }

    private void _UpdateSizeTransition() {
        if (!_isSizeTransitionActive) return;

        _sizeTransitionTimer += Time.deltaTime;
        if (_sizeTransitionTimer >= _sizeTransition.duration) {
            _camera.orthographicSize = _currentSize = _sizeTransitionEnd;
            _isSizeTransitionActive = false;
        } else {
            float r = _sizeTransitionTimer / _sizeTransition.duration;
            if (null != _sizeTransition.curve) {
                r = _sizeTransition.curve.Evaluate(r);
            }

            _camera.orthographicSize = _currentSize = Mathf.Lerp(_sizeTransitionStart, _sizeTransitionEnd, r);
        }
    }

    private void _UpdateMoveTransition() {
        if (!_isMoveTransitonActive) return;

        _moveTransitionTimer += Time.deltaTime;
        if (_moveTransitionTimer >= _moveTransition.duration) {
            transform.localPosition = _moveTransitionDestination;
            _isMoveTransitonActive = false;
        } else {
            float r = _moveTransitionTimer / _moveTransition.duration;
            if (null != _moveTransition.curve) {
                r = _moveTransition.curve.Evaluate(r);
            }

            transform.localPosition = Vector3.Lerp(_moveTransitionOrigin, _moveTransitionDestination, r);
        }
    }

    private void _UpdateRotateTransition() {
        if (!_isRotateTransitonActive) return;

        _rotateTransitionTimer += Time.deltaTime;
        if (_rotateTransitionTimer >= _rotateTransition.duration) {
            transform.localEulerAngles = _rotateTransitionDestination;
            _isRotateTransitonActive = false;
        } else {
            float r = _rotateTransitionTimer / _rotateTransition.duration;
            if (null != _rotateTransition.curve) {
                r = _rotateTransition.curve.Evaluate(r);
            }

            transform.localEulerAngles = Vector3.Lerp(_rotateTransitionOrigin, _rotateTransitionDestination, r);
        }
    }

    //private void _UpdateZoomTransition() {
    //    if(!_isZoomActive) return;

    //    _zoomTimer += Time.deltaTime;
    //    if (_zoomTimer >= _zoom.duration) {
    //        transform.position = _zoomDestination;
    //        _isZoomActive = false;
    //    } else {
    //        float ratio = _zoomTimer / _zoom.duration;
    //        if(_zoom.curve != null) {
    //            ratio = _zoom.curve.Evaluate(ratio);
    //        }

    //        transform.position = Vector3.Lerp(_zoomOrigin, _zoomDestination, ratio);
    //    }
    //}

    public void Zoom(Vector3 destination, float percentage, CameraTransition transition = null) {
        if (percentage < 0) {
            destination = Vector3.Lerp(transform.position, transform.position - (destination - transform.position), Mathf.Abs(percentage));
        } else {
            destination = Vector3.Lerp(transform.position, destination, percentage);
        }
        MoveTo(destination, PROFILE_MODE.OVERRIDE, transition);
    }
}
