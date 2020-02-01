using UnityEngine;

public sealed class BrokenItem : MonoBehaviour {

    [SerializeField]
    private GameObject _goal;

    private bool _isGoalAchieved;
    public bool IsGoalAchieved => _isGoalAchieved;

    private Vector3 _startPosition;
    private Quaternion _startRotation;


    private void Awake() {
        _startPosition = transform.position;
        _startRotation = transform.rotation;
    }

    private void OnEnable() {
        _isGoalAchieved = false;
        _goal.SetActive(true);

        transform.position = _startPosition;
        transform.rotation = _startRotation;
    }

    private void OnDisable() {
        _goal.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject == _goal) {
            _isGoalAchieved = true;
        }
    }
}
