using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    private GameManager _gameManager;

    [SerializeField]
    private Transform _centerPos;
    [SerializeField]
    private Transform _leftPos;
    [SerializeField]
    private Transform _rightPos;

    [SerializeField]
    private float _moveTime;
    [SerializeField]
    private float _jumpValue;
    [SerializeField]
    private float _jumpTime;

    enum Lane { LEFT, CENTER, RIGHT }

    private Lane _lane = Lane.CENTER;

    private bool _isGrounded = true;

    private void Awake() {
        _gameManager = FindObjectOfType<GameManager>();
    }

    public void MoveToLeft() {
        if (_isGrounded) {
            if (_lane == Lane.RIGHT) {
                _lane = Lane.CENTER;
                transform.DOMove(_centerPos.position, _moveTime).SetEase(Ease.Linear);
            } else if (_lane == Lane.CENTER) {
                _lane = Lane.LEFT;
                transform.DOMove(_leftPos.position, _moveTime).SetEase(Ease.Linear);
            }
        }
    }

    //public void MoveToCenter() {
    //    if (_lane == Lane.LEFT || _lane == Lane.RIGHT) {
    //        _lane = Lane.CENTER;
    //        transform.DOMove(_centerPos.position, _moveSpeed);
    //    }
    //}

    public void MoveToRight() {
        if (_isGrounded) {
            if (_lane == Lane.LEFT) {
                _lane = Lane.CENTER;
                transform.DOMove(_centerPos.position, _moveTime).SetEase(Ease.Linear);
            } else if (_lane == Lane.CENTER) {
                _lane = Lane.RIGHT;
                transform.DOMove(_rightPos.position, _moveTime).SetEase(Ease.Linear);
            }
        }
    }

    public void Jump() {
        if ( _isGrounded ) {
            switch (_lane) {
                case Lane.LEFT:
                    transform.DOJump(_leftPos.position, _jumpValue, 1, _jumpTime).SetEase(Ease.Linear).OnComplete(() => _isGrounded = true);
                    break;
                case Lane.CENTER:
                    transform.DOJump(_centerPos.position, _jumpValue, 1, _jumpTime).SetEase(Ease.Linear).OnComplete(() => _isGrounded = true);
                    break;
                case Lane.RIGHT:
                    transform.DOJump(_rightPos.position, _jumpValue, 1, _jumpTime).SetEase(Ease.Linear).OnComplete(() => _isGrounded = true);
                    break;
                default:
                    break;
            }
            _isGrounded = false;
        }
    }

    public void HitByObstacle() {
        Debug.Log("hit by obstacle");
        _gameManager.Lose();
    }

    public void HitByBonus() {
        Debug.Log("hit by bonus");
    }
}
