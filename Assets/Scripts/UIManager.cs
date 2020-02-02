using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public sealed class UIManager : MonoBehaviour {

    [SerializeField]
    private EventSystem _eventSystem;


    [SerializeField]
    private float _waitingDelay = 0.5f;


    [SerializeField]
    private GameObject _victoryRoot;

    [SerializeField]
    private Button _victoryRestartButton;


    [SerializeField]
    private GameObject _defeatRoot;

    [SerializeField]
    private Button _defeatRestartButton;


    [SerializeField]
    private GameObject _timerRoot;

    [SerializeField]
    private Text _timerText;


    [SerializeField]
    private CanvasGroup _hintsGroup;

    [SerializeField]
    private float _hintsFadeSpeed = 10f;


    private void Start() {
        _victoryRestartButton.onClick.AddListener(GameLogic.Instance.RestartGame);
        _defeatRestartButton.onClick.AddListener(GameLogic.Instance.RestartGame);
    }

    private void Update() {
        _victoryRoot.SetActive(false);
        _defeatRoot.SetActive(false);
        _timerRoot.SetActive(false);

        switch (GameLogic.Instance.state) {
            case GameLogic.State.Waiting:
                _timerRoot.SetActive(true);
                var isButtonPressed = Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) ||
                                      Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow);
                if (isButtonPressed && GameLogic.Instance.StateSetTime + _waitingDelay < Time.timeSinceLevelLoad) {
                    GameLogic.Instance.SetState(GameLogic.State.Playing);
                }

                _hintsGroup.alpha += _hintsFadeSpeed * Time.deltaTime;
                break;
            case GameLogic.State.Playing:
                _timerRoot.SetActive(true);

                _hintsGroup.alpha -= _hintsFadeSpeed * Time.deltaTime;
                break;
            case GameLogic.State.Victory:
                //if (GameLogic.Instance.IsVictoryAnimationFinished || GameLogic.Instance.IsReplayFinished()) {
                    _victoryRoot.SetActive(true);
                    if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Return)) {
                        _victoryRestartButton.OnPointerClick(new PointerEventData(_eventSystem));
                    }
                //}

                _hintsGroup.alpha -= _hintsFadeSpeed * Time.deltaTime;
                break;
            case GameLogic.State.Defeat:
                _defeatRoot.SetActive(true);
                if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Return)) {
                    _defeatRestartButton.OnPointerClick(new PointerEventData(_eventSystem));
                }

                _hintsGroup.alpha -= _hintsFadeSpeed * Time.deltaTime;
                break;
        }

        _timerText.text = string.Format("{0:0}:{1:00}", GameLogic.Instance.LeftTime / 60, GameLogic.Instance.LeftTime % 60);
    }
}