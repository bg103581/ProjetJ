using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    private bool _swipeAvailable = true;
    private Vector2 _startTouchPos;
    private Vector2 _currentTouchPos;
    private Vector2 _swipe;
    private Touch _touch;

    [SerializeField]
    private float distanceSwipe = 10f;

    [SerializeField]
    private Player _player;

    private void Update() {

        if (Input.touchCount > 0) {
            _touch = Input.GetTouch(0);

            if (_touch.phase == TouchPhase.Began) {
                _startTouchPos = _touch.position;
                _swipeAvailable = true;
            }

            _currentTouchPos = _touch.position;
            _swipe = _currentTouchPos - _startTouchPos;
            float angle = Vector2.SignedAngle(Vector2.right, _swipe);  //angle between right and swipe to know if swipe right, up or left

            //swipe
            if (_swipe.magnitude > distanceSwipe && _swipeAvailable) {
                if (Mathf.Abs(angle) <= 45f) {
                    //swipe right
                    _player.MoveToRight();
                    _swipeAvailable = false;
                } else if (angle > 45f && angle <= 135f) {
                    //swipe up
                    _player.Jump();
                    _swipeAvailable = false;
                } else if (Mathf.Abs(angle) > 135f) {
                    //swipe left
                    _player.MoveToLeft();
                    _swipeAvailable = false;
                }
            }
        }
    }
}
