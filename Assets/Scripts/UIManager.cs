using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public sealed class UIManager : MonoBehaviour {

    [SerializeField]
    private EventSystem _eventSystem;


    [SerializeField]
    private GameObject _waitingRoot;

    [SerializeField]
    private float _waitingDelay = 0.5f;


    [SerializeField]
    private GameObject _playingRoot;

    [SerializeField]
    private Text _timerText;


    [SerializeField]
    private GameObject _victoryRoot;

    [SerializeField]
    private Button _victoryRestartButton;


    [SerializeField]
    private GameObject _defeatRoot;

    [SerializeField]
    private Button _defeatRestartButton;


    private void Start() {
        _victoryRestartButton.onClick.AddListener(GameLogic.Instance.RestartGame);
        _defeatRestartButton.onClick.AddListener(GameLogic.Instance.RestartGame);
    }

    private void Update() {
        _waitingRoot.SetActive(false);
        _playingRoot.SetActive(false);
        _victoryRoot.SetActive(false);
        _defeatRoot.SetActive(false);

        switch (GameLogic.Instance.state) {
            case GameLogic.State.Waiting:
                _waitingRoot.SetActive(true);
                var isButtonPressed = Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) ||
                                      Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow);
                if (isButtonPressed && GameLogic.Instance.StateSetTime + _waitingDelay < Time.timeSinceLevelLoad) {
                    GameLogic.Instance.SetState(GameLogic.State.Playing);
                }
                break;
            case GameLogic.State.Playing:
                _playingRoot.SetActive(true);
                break;
            case GameLogic.State.Victory:
                _victoryRoot.SetActive(true);
                if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Return)) {
                    _victoryRestartButton.OnPointerClick(new PointerEventData(_eventSystem));
                }
                break;
            case GameLogic.State.Defeat:
                _defeatRoot.SetActive(true);
                if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Return)) {
                    _defeatRestartButton.OnPointerClick(new PointerEventData(_eventSystem));
                }
                break;
        }

        _timerText.text = string.Format("{0:0}:{1:00}", GameLogic.Instance.LeftTime / 60, GameLogic.Instance.LeftTime % 60);
    }
}