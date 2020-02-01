using UnityEngine;

public sealed class GameController : MonoBehaviour {

    [SerializeField]
    private Rigidbody2D _target;

    [SerializeField]
    private float _force;

    private void Update() {
        if (Input.GetKey(KeyCode.UpArrow)) {
            _target.AddForce(new Vector2(0f, _force * 2f));
        }
        if (Input.GetKey(KeyCode.DownArrow)) {
            _target.AddForce(new Vector2(0f, -_force));
        }
        if (Input.GetKey(KeyCode.LeftArrow)) {
            _target.AddForce(new Vector2(-_force, 0f));
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
            _target.AddForce(new Vector2(_force, 0f));
        }

    }
}