using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    private bool _swipeAvailable = true;
    [HideInInspector]
    public bool isRegisteringInputs = false;
    [HideInInspector]
    public bool isAlienClickable = false;

    private Vector2 _startTouchPos;
    private Vector2 _currentTouchPos;
    private Vector2 _swipe;

    private Touch _touch;

    [HideInInspector]
    public Vector3 alienScreenPos;

    private float leftAlienMargin;
    private float rightAlienMargin;
    private float upAlienMargin;
    private float downAlienMargin;
    
    private SpawnAlien spawnAlien;
    private GameManager gameManager;

    [SerializeField]
    private float distanceSwipe = 10f;
    [SerializeField]
    private float alienTouchMargin;
    [SerializeField]
    private Player _player;
    [SerializeField]
    private bool keyboardControl;

    private void Awake() {
        spawnAlien = FindObjectOfType<SpawnAlien>();
        gameManager = FindObjectOfType<GameManager>();

        GameEvents.current.onReplayButtonTrigger += OnReset;
        GameEvents.current.onMainMenuButtonTrigger += OnReset;
    }

    private void Update() {
        if (gameManager.gameState == GameState.PLAYING && isRegisteringInputs) {
            if (keyboardControl) {
                if (Input.GetKeyDown(KeyCode.LeftArrow)) _player.MoveToLeft();
                if (Input.GetKeyDown(KeyCode.RightArrow)) _player.MoveToRight();
                if (Input.GetKeyDown(KeyCode.UpArrow)) _player.Jump();

                if (isAlienClickable) {
                    if (Input.GetKeyDown(KeyCode.Space)) spawnAlien.TouchAlien();
                }
            }
            else {
                if (Input.touchCount > 0) {
                    _touch = Input.GetTouch(0);

                    if (_touch.phase == TouchPhase.Began) {
                        _startTouchPos = _touch.position;
                        _swipeAvailable = true;

                        if (isAlienClickable) {
                            leftAlienMargin = alienScreenPos.x - alienTouchMargin;
                            rightAlienMargin = alienScreenPos.x + alienTouchMargin;
                            upAlienMargin = alienScreenPos.y + alienTouchMargin;
                            downAlienMargin = alienScreenPos.y - alienTouchMargin;
                            if (_startTouchPos.x >= leftAlienMargin && _startTouchPos.x <= rightAlienMargin &&
                                _startTouchPos.y <= upAlienMargin && _startTouchPos.y >= downAlienMargin) {
                                spawnAlien.TouchAlien();
                            }
                        }
                    }

                    _currentTouchPos = _touch.position;
                    _swipe = _currentTouchPos - _startTouchPos;
                    float angle = Vector2.SignedAngle(Vector2.right, _swipe);  //angle between right and swipe to know if swipe right, up or left
                    

                    //swipe
                    if (_swipe.magnitude > distanceSwipe && _swipeAvailable) {
                        Debug.Log(angle);
                        if (Mathf.Abs(angle) <= 45f) {
                            //swipe right
                            _player.MoveToRight();
                            _swipeAvailable = false;
                        }
                        else if (angle > 45f && angle <= 135f) {
                            //swipe up
                            _player.Jump();
                            _swipeAvailable = false;
                        }
                        else if (Mathf.Abs(angle) > 135f) {
                            //swipe left
                            _player.MoveToLeft();
                            _swipeAvailable = false;
                        }
                    }
                }
            }
        }
        else if (gameManager.gameState == GameState.WAITING) {
            if (keyboardControl) {
                if (Input.GetKeyDown(KeyCode.Return)) gameManager.StartAnimation();
            }
            else {
                if (Input.touchCount > 0) {
                    _touch = Input.GetTouch(0);

                    if (_touch.phase == TouchPhase.Began) {
                        gameManager.StartAnimation();
                    }
                }
            }
        }
    }

    private void OnDestroy() {
        GameEvents.current.onReplayButtonTrigger -= OnReset;
        GameEvents.current.onMainMenuButtonTrigger -= OnReset;
    }

    private void OnReset() {
        isRegisteringInputs = false;
    }
}
