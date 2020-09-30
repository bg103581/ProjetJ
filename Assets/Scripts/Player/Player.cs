using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField]
    private Transform centerPos;
    [SerializeField]
    private Transform leftPos;
    [SerializeField]
    private Transform rightPos;

    [SerializeField]
    private float moveTime;
    [SerializeField]
    private float jumpValue;
    [SerializeField]
    private float jumpTime;
    [SerializeField]
    private float dodgeStreakTime;
    
    private Lane lane = Lane.CENTER;

    private bool isGrounded = true;
    private bool isDodgeStreak = false;
    private bool startDodgeStreakTimer = false;

    private float dodgeStreakTimer;

    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update() {
        if (startDodgeStreakTimer) {
            isDodgeStreak = true;
            dodgeStreakTimer = dodgeStreakTime;

            startDodgeStreakTimer = false;
        }

        if (isDodgeStreak) {
            if (dodgeStreakTimer <= 0f) {
                isDodgeStreak = false;
            }
            else {
                dodgeStreakTimer -= Time.deltaTime;
            }
        }
    }

    public void MoveToLeft() {
        if (lane == Lane.RIGHT) {
            lane = Lane.CENTER;
            transform.DOMoveX(centerPos.position.x, moveTime).SetEase(Ease.Linear);
        }
        else if (lane == Lane.CENTER) {
            lane = Lane.LEFT;
            transform.DOMoveX(leftPos.position.x, moveTime).SetEase(Ease.Linear);
        }
    }

    public void MoveToRight() {
        if (lane == Lane.LEFT) {
            lane = Lane.CENTER;
            transform.DOMoveX(centerPos.position.x, moveTime).SetEase(Ease.Linear);
        }
        else if (lane == Lane.CENTER) {
            lane = Lane.RIGHT;
            transform.DOMoveX(rightPos.position.x, moveTime).SetEase(Ease.Linear);
        }
    }

    public void Jump() {
        if (isGrounded) {
            isGrounded = false;

            Sequence sequence = DOTween.Sequence();

            sequence.Append(transform.DOMoveY(transform.position.y + 3, jumpTime/2).SetEase(Ease.OutSine));
            sequence.Append(transform.DOMoveY(transform.position.y, jumpTime/2).SetEase(Ease.InSine));
            sequence.OnComplete(() => isGrounded = true);

            sequence.Play();
        }
    }

    public void HitByObstacle() {
        Debug.Log("hit by obstacle");
        gameManager.Lose();
    }

    public void HitByBonus() {
        Debug.Log("hit by bonus");
    }

    public void HitByDisc(bool isGold) {
        Debug.Log("hit by disc");
        gameManager.AddDiscScore(isGold);
    }

    public void DodgeObstacle() {
        startDodgeStreakTimer = true;

        gameManager.AddDodgeScore(isDodgeStreak);
    }
}
