using UnityEngine;

public sealed class GameLogic : MonoBehaviour {

    [SerializeField]
    private BrokenItem[] _items;

    [SerializeField]
    private Controller _controller;


    private int _tick;
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

            _tick = 0;

            SetupBrokenItem();
        }
    }

    private void FixedUpdate() {
        for (int i = 0; i < _items.Length; i++) {
            _items[i].OnFixedUpdate(_tick);
        }
        _tick++;
    }

    private void SetupBrokenItem() {
        for (int i = 0; i < _items.Length; i++) {
            _items[i].SetState(i == _currentItem ? BrokenItem.State.Active :
                               i < _currentItem ? BrokenItem.State.Replay : BrokenItem.State.Disabled);
        }
        _controller.SetTarget(_items[_currentItem].RB);
    }
}