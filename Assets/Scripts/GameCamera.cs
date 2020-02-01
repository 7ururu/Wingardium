using UnityEngine;

public sealed class GameCamera : MonoBehaviour {

    [SerializeField]
    private float _overviewSize = 5f;

    [SerializeField]
    private float _followSize = 2.5f;

    [SerializeField]
    private float _smoothTime = 0.15f;

    [SerializeField]
    private float _positionSmoothMaxSpeed = 1f;

    [SerializeField]
    private float _sizeSmoothMaxSpeed = 2f;


    private Transform _target;
    public Transform Target {
        get => _target;
        set {
            if (_target != value) {
                _positionSmoothVelocity = Vector2.zero;
                _sizeSmoothVelocity = 0f;
            }
            _target = value;
        }
    }


    private Camera _camera;

    private Vector2 _positionSmoothVelocity;
    private float _sizeSmoothVelocity;


    private void Start() {
        _camera = GetComponent<Camera>();
    }

    private void LateUpdate() {
        var targetPosition = Vector2.zero;
        var targetSize = _overviewSize;

        if (Target != null) {
            targetSize = _followSize;
            targetPosition = new Vector2(Target.position.x, Target.position.y);
        }

        var position = new Vector2(transform.position.x, transform.position.y);
        position = Vector2.SmoothDamp(position, targetPosition, ref _positionSmoothVelocity, _smoothTime, _positionSmoothMaxSpeed, Time.deltaTime);
        transform.position = new Vector3(position.x, position.y, transform.position.z);

        _camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, targetSize, ref _sizeSmoothVelocity, _smoothTime, _sizeSmoothMaxSpeed, Time.deltaTime);
    }
}