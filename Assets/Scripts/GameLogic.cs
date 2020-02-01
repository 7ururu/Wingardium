using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class GameLogic : MonoBehaviour {

    private static GameLogic _instance;
    public static GameLogic Instance => _instance;


    [SerializeField]
    private BrokenItem[] _items;

    [SerializeField]
    private Controller _controller;

    [SerializeField]
    private GameCamera _gameCamera;

    [SerializeField]
    private int _gameTime = 75;

    [SerializeField]
    private float _itemSwitchDelay;



    public enum State {

        Waiting,
        Playing,
        Victory,
        Defeat,
    }
    private State _state;
    public State state {
        get => _state;
        set {
            _state = value;
            _stateSetTime = Time.timeSinceLevelLoad;
        }
    }
    private float _stateSetTime;
    public float StateSetTime => _stateSetTime;


    private int _tick;
    private int _currentItem;
    private float _gameTimer;
    public int LeftTime => Mathf.Max(0, _gameTime - (int)_gameTimer);


    public void SetState(State state) {
        this.state = state;
        _tick = 0;
        _controller.SetTarget(null);
        switch (_state) {
            case State.Waiting:
                for (int i = 0; i < _items.Length; i++) {
                    _items[i].SetState(i == _currentItem ? BrokenItem.State.Preview : 
                                       i < _currentItem ? BrokenItem.State.OnPlace : BrokenItem.State.Disabled);
                }
                _gameCamera.Target = null;
                break;
            case State.Playing:
                for (int i = 0; i < _items.Length; i++) {
                    _items[i].SetState(i == _currentItem ? BrokenItem.State.Active :
                                       i < _currentItem ? BrokenItem.State.Replay : BrokenItem.State.Disabled);
                }
                _controller.SetTarget(_items[_currentItem].RB);
                _gameCamera.Target = _items[_currentItem].transform;
                break;
            case State.Victory:
                for (int i = 0; i < _items.Length; i++) {
                    _items[i].SetState(BrokenItem.State.Replay);
                }
                _gameCamera.Target = null;
                break;
            case State.Defeat:
                for (int i = 0; i < _items.Length; i++) {
                    _items[i].SetState(BrokenItem.State.Disabled);
                }
                _gameCamera.Target = null;
                break;
        }
    }

    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public bool IsReplayFinished() {
        for (int i = 0; i < _items.Length; i++) {
            if (!_items[i].IsReplayFinished(_tick)) {
                return false;
            }
        }
        return true;
    }


    private void Awake() {
        _instance = this;
    }

    private void Start() {
        SetState(State.Waiting);
    }

    private void Update() {
        switch (_state) {
            case State.Playing:
                _gameTimer += Time.deltaTime;
                if (_gameTimer >= _gameTime) {
                    SetState(State.Defeat);
                } else if (_items[_currentItem].IsGoalAchieved && _items[_currentItem].GoalAchieveTime + _itemSwitchDelay < Time.timeSinceLevelLoad) {
                    _currentItem++;
                    if (_currentItem == _items.Length) {
                        SetState(State.Victory);
                    } else {
                        SetState(State.Waiting);
                    }
                }
                break;
        }        
    }

    private void FixedUpdate() {
        for (int i = 0; i < _items.Length; i++) {
            _items[i].OnFixedUpdate(_tick);
        }
        _tick++;
    }
}