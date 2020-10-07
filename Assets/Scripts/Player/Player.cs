using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    private GameManager gameManager;
    private CameraMovement cameraMovement;

    [SerializeField]
    private Transform centerPos;
    [SerializeField]
    private Transform leftPos;
    [SerializeField]
    private Transform rightPos;

    [SerializeField]
    private float moveTime;
    [SerializeField]
    private float jumpVelocity;
    [SerializeField]
    private float jumpMultiplier;
    [SerializeField]
    private float dodgeStreakTime;
    [SerializeField]
    private float fallMultiplier;
    
    private Lane lane = Lane.CENTER;

    [HideInInspector]
    public bool isGrounded = true;
    private bool isDodgeStreak = false;
    private bool startDodgeStreakTimer = false;
    [HideInInspector]
    public bool isOvni = false;
    private bool isClaquettes = false;
    private bool startClaquettesTimer = false;

    private float dodgeStreakTimer;
    private float claquettesTimer;

    private Rigidbody rb;

    [Header("Bonus variables")]
    [SerializeField]
    private float claquettesJumpVelocity;
    [SerializeField]
    private float claquettesDuration;

    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();
        rb = GetComponent<Rigidbody>();
        cameraMovement = FindObjectOfType<CameraMovement>();
    }

    private void Update() {
        DodgeStreakTimerUpdate();  //dodge streak timer
        ClaquettesTimerUpdate();   //claquettes timer

        if (rb.velocity.y < 0) {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0) {
            rb.velocity += Vector3.up * Physics.gravity.y * (jumpMultiplier - 1) * Time.deltaTime;
        }
    }

    public void MoveToLeft() {
        if (lane == Lane.RIGHT) {
            lane = Lane.CENTER;
            transform.DOMoveX(centerPos.position.x, moveTime).SetEase(Ease.Linear).OnComplete(() => rb.velocity = new Vector3(0, rb.velocity.y));
            cameraMovement.MoveToCenterPos(moveTime);
        }
        else if (lane == Lane.CENTER) {
            lane = Lane.LEFT;
            transform.DOMoveX(leftPos.position.x, moveTime).SetEase(Ease.Linear).OnComplete(() => rb.velocity = new Vector3(0, rb.velocity.y));
            cameraMovement.MoveToLeftPos(moveTime);
        }
    }

    public void MoveToRight() {
        if (lane == Lane.LEFT) {
            lane = Lane.CENTER;
            transform.DOMoveX(centerPos.position.x, moveTime).SetEase(Ease.Linear).OnComplete(() => rb.velocity = new Vector3(0, rb.velocity.y));
            cameraMovement.MoveToCenterPos(moveTime);
        }
        else if (lane == Lane.CENTER) {
            lane = Lane.RIGHT;
            transform.DOMoveX(rightPos.position.x, moveTime).SetEase(Ease.Linear).OnComplete(() => rb.velocity = new Vector3(0, rb.velocity.y));
            cameraMovement.MoveToRightPos(moveTime);
        }
    }

    public void Jump() {
        if (isGrounded) {
            isGrounded = false;

            //Sequence sequence = DOTween.Sequence();

            //sequence.Append(rb.DOMoveY(transform.position.y + 3, jumpTime/2).SetEase(Ease.OutSine));
            ////sequence.Append(transform.DOMoveY(transform.position.y, jumpTime/2).SetEase(Ease.InSine));
            //sequence.OnComplete(() => isGrounded = true);

            //sequence.Play();
            if (isClaquettes)
                rb.velocity = Vector3.up * claquettesJumpVelocity;
            else
                rb.velocity = Vector3.up * jumpVelocity;
        }
    }

    public void HitByObstacle() {
        Debug.Log("hit by obstacle");
        gameManager.Lose();
    }

    public void HitByBonus(string tag) {
        Debug.Log("hit by bonus");
        switch (tag) {
            case "Claquettes":
                startClaquettesTimer = true;
                break;
            case "Pochon":
                break;
            case "Twingo":
                break;
            case "Tmax":
                break;
            case "Ovni":
                break;
            default:
                break;
        }
    }

    public void HitByDisc(bool isGold) {
        gameManager.AddDiscScore(isGold);
    }

    public void DodgeObstacle() {
        startDodgeStreakTimer = true;

        gameManager.AddDodgeScore(isDodgeStreak);
    }

    private void DodgeStreakTimerUpdate() {
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

    private void ClaquettesTimerUpdate() {
        if (startClaquettesTimer) {
            isClaquettes = true;
            claquettesTimer = claquettesDuration;

            startClaquettesTimer = false;
        }

        if (isClaquettes) {
            if (claquettesTimer <= 0f) {
                isClaquettes = false;
            }
            else {
                claquettesTimer -= Time.deltaTime;
            }
        }
    }
}
