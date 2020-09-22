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
        if (_lane == Lane.RIGHT) {
            _lane = Lane.CENTER;
            transform.DOMoveX(_centerPos.position.x, _moveTime).SetEase(Ease.Linear);
        }
        else if (_lane == Lane.CENTER) {
            _lane = Lane.LEFT;
            transform.DOMoveX(_leftPos.position.x, _moveTime).SetEase(Ease.Linear);
        }
    }

    //public void MoveToCenter() {
    //    if (_lane == Lane.LEFT || _lane == Lane.RIGHT) {
    //        _lane = Lane.CENTER;
    //        transform.DOMove(_centerPos.position, _moveSpeed);
    //    }
    //}

    public void MoveToRight() {
        if (_lane == Lane.LEFT) {
            _lane = Lane.CENTER;
            transform.DOMoveX(_centerPos.position.x, _moveTime).SetEase(Ease.Linear);
        }
        else if (_lane == Lane.CENTER) {
            _lane = Lane.RIGHT;
            transform.DOMoveX(_rightPos.position.x, _moveTime).SetEase(Ease.Linear);
        }
    }

    public void Jump() {
        if (_isGrounded) {
            _isGrounded = false;

            Sequence sequence = DOTween.Sequence();

            sequence.Append(transform.DOMoveY(transform.position.y + 3, _jumpTime/2).SetEase(Ease.OutSine));
            sequence.Append(transform.DOMoveY(transform.position.y, _jumpTime/2).SetEase(Ease.InSine));
            sequence.OnComplete(() => _isGrounded = true);

            sequence.Play();
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
