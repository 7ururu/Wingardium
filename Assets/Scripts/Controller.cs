using UnityEngine;

public sealed class Controller : MonoBehaviour {

    [SerializeField]
    private Rigidbody2D _target;

    [SerializeField]
    private float _force;


    private Vector2 _resultForce;


    public void SetTarget(Rigidbody2D target) {
        _target = target;
        _resultForce = Vector2.zero;
    }

    private void Update() {
        _resultForce = Vector2.zero;
        if (_target == null) {
            return;
        }

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
            _resultForce += new Vector2(0f, _force * 2f);
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
            _resultForce += new Vector2(0f, -_force);
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
            _resultForce += new Vector2(-_force, 0f);
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
            _resultForce += new Vector2(_force, 0f);
        }
    }

    private void FixedUpdate() {
        if (_target == null) {
            return;
        }
        
        _target.AddForce(_resultForce * Time.fixedDeltaTime);
    }
}