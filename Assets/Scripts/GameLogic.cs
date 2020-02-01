using UnityEngine;

public sealed class GameLogic : MonoBehaviour {

    [SerializeField]
    private BrokenItem[] _items;

    [SerializeField]
    private Controller _controller;


    private int _currentItem;


    private void Start() {
        SetupBrokenItem();
    }

    private void Update() {
        if (_items[_currentItem].IsGoalAchieved) {
            _currentItem++;
            if (_currentItem == _items.Length) {
                Debug.Log("Victory!");
                _currentItem = 0;
            }
            SetupBrokenItem();
        }
    }

    private void SetupBrokenItem() {
        for (int i = 0; i < _items.Length; i++) {
            _items[i].gameObject.SetActive(i == _currentItem);
        }
        _controller.SetTarget(_items[_currentItem].GetComponent<Rigidbody2D>());
    }
}