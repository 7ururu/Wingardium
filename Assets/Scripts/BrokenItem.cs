﻿using System.Collections.Generic;
using UnityEngine;

public sealed class BrokenItem : MonoBehaviour {

    [SerializeField]
    private Transform _goal;

    [SerializeField]
    private ParticleSystem _goalParticlesPrefab;


    [SerializeField]
    private float _goalSmoothTime;

    [SerializeField]
    private float _goalSmoothPositionMaxSpeed;

    [SerializeField]
    private float _goalSmoothRotationMaxSpeed;


    private bool _isGoalAchieved;
    public bool IsGoalAchieved => _isGoalAchieved;

    private float _goalAchieveTime;
    public float GoalAchieveTime => _goalAchieveTime;

    public enum State {

        Disabled,
        Active,
        Replay,
        Preview,
    }
    private State _state;

    private Rigidbody2D _rb;
    public Rigidbody2D RB => _rb;

    private Vector3 _startPosition;
    private Quaternion _startRotation;

    private List<Vector3> _positions = new List<Vector3>();
    private List<Quaternion> _rotations = new List<Quaternion>();
    private List<Vector2> _velocities = new List<Vector2>();
    private List<float> _angularVelocities = new List<float>();

    private ParticleSystem _goalParticlesInstance;


    private Vector3 _goalSmoothPositionVelocity;
    private float _goalSmoothRotationVelocity;
    


    public void SetState(State state) {
        _state = state;

        _rb.velocity = Vector2.zero;
        _rb.angularVelocity = 0f;

        _goalSmoothPositionVelocity = Vector3.zero;
        _goalSmoothRotationVelocity = 0f;

        var goalEmission = _goalParticlesInstance.emission;

        switch (_state) {
            case State.Disabled:
                gameObject.SetActive(false);
                goalEmission.enabled = false;
                break;

            case State.Active:
                _isGoalAchieved = false;

                gameObject.SetActive(true);
                goalEmission.enabled = true;
                ResetTransform();
                _rb.isKinematic = false;

                _positions.Clear();
                _rotations.Clear();
                _velocities.Clear();
                _angularVelocities.Clear();
                break;

            case State.Replay:
                gameObject.SetActive(true);
                goalEmission.enabled = false;
                ResetTransform();
                _rb.isKinematic = true;
                break;

            case State.Preview:
                gameObject.SetActive(true);
                goalEmission.enabled = true;
                ResetTransform();
                _rb.isKinematic = true;
                break;
        }
    }

    public void OnFixedUpdate(int tick) {
        switch (_state) {
            case State.Active:
                _positions.Add(transform.position);
                _rotations.Add(transform.rotation);
                _velocities.Add(_rb.velocity);
                _angularVelocities.Add(_rb.angularVelocity);
                break;
            case State.Replay:
                var index = Mathf.Min(_positions.Count - 1, tick);
                _rb.MovePosition(_positions[index]);
                _rb.MoveRotation(_rotations[index]);
                _rb.velocity = _velocities[index];
                _rb.angularVelocity = _angularVelocities[index];
                break;
        }
    }

    private void Update() {
        if (_state == State.Active && _isGoalAchieved) {
            transform.position = Vector3.SmoothDamp(transform.position, _goal.position, ref _goalSmoothPositionVelocity,
                                                    _goalSmoothTime, _goalSmoothPositionMaxSpeed, Time.deltaTime);
            transform.eulerAngles = new Vector3(0f, 0f,
                Mathf.SmoothDampAngle(transform.eulerAngles.z, _goal.eulerAngles.z, ref _goalSmoothRotationVelocity,
                                      _goalSmoothTime, _goalSmoothRotationMaxSpeed, Time.deltaTime));
        }
    }

    private void Awake() {
        _startPosition = transform.position;
        _startRotation = transform.rotation;

        _rb = GetComponent<Rigidbody2D>();


        _goalParticlesInstance = Instantiate(_goalParticlesPrefab);
        _goalParticlesInstance.transform.localPosition = _goal.position;

        var emission = _goalParticlesInstance.emission;
        emission.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.transform == _goal) {
            _isGoalAchieved = true;
            _goalAchieveTime = Time.timeSinceLevelLoad;

            _rb.isKinematic = true;
            _rb.velocity = Vector2.zero;
            _rb.angularVelocity = 0f;
        }
    }

    private void ResetTransform() {
        transform.position = _startPosition;
        transform.rotation = _startRotation;
    }
}
