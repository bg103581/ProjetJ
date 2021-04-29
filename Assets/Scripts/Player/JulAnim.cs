using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class JulAnim : MonoBehaviour
{

    [SerializeField]
    private float moveXTime = 0.5f;
    [SerializeField]
    private float rotateRunTime = 0.5f;
    [SerializeField]
    private float rotatePlayPosTime = 0.5f;
    [SerializeField]
    private GameObject petou;

    private Vector3 startPos;
    private Quaternion startRot;

    private Animator anim;
    private GameManager gameManager;

    private Transform julTransform = null;

    private void Awake() {
        julTransform = transform;
        startPos = julTransform.localPosition;
        startRot = julTransform.localRotation;
        anim = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();

        GameEvents.current.onReplayButtonTrigger += OnReplay;
        GameEvents.current.onMainMenuButtonTrigger += OnMainMenu;
        GameEvents.current.onContinueGame += OnContinue;
        GameEvents.current.onPreContinueGame += OnPreContinue;
    }
    
    private void OnDestroy() {
        GameEvents.current.onReplayButtonTrigger -= OnReplay;
        GameEvents.current.onMainMenuButtonTrigger -= OnMainMenu;
        GameEvents.current.onContinueGame -= OnContinue;
        GameEvents.current.onPreContinueGame -= OnPreContinue;
    }

    private void OnEnable() {
        if (gameManager.gameState == GameState.PLAYING)
         anim.Play("Run");
    }

    private void OnReplay() {
        julTransform.localPosition = startPos;
        julTransform.localRotation = startRot;

        anim.Play("Idle");
    }

    private void OnMainMenu()
    {
        petou.SetActive(true);
        julTransform.localPosition = startPos;
        julTransform.localRotation = startRot;

        anim.Play("Idle");
    }

    private void OnContinue()
    {
        anim.Play("Run");
    }

    private void OnPreContinue()
    {
        anim.Play("Idle");
    }

    public void PlayStartMovement() {
        petou.SetActive(false);

        Sequence mySequence = DOTween.Sequence();

        mySequence.Append(julTransform.DOMoveX(0f, moveXTime).SetEase(Ease.OutSine));
        mySequence.Join(julTransform.DORotate(new Vector3(0f, 0f, 0f), rotateRunTime).SetEase(Ease.OutSine));

        mySequence.Play();
    }

    public void RotatePlayPos() {
        julTransform.DOLocalRotate(new Vector3(0f, -1f ,0f) , rotatePlayPosTime);
    }

    public void Strafe() {
        anim.SetTrigger("strafeTrigger");
    }

    public void Jump() {
        anim.SetTrigger("jumpTrigger");
        SetFallBool(true);
    }

    public void StartAnimation() {
        anim.SetTrigger("startAnimationTrigger");
    }

    public void Hit() {
        anim.SetTrigger("hitTrigger");
    }

    public void Death() {
        anim.SetTrigger("deathTrigger");
    }

    public void SetFallBool(bool isFalling) {
        anim.SetBool("isFalling", isFalling);
    }

    public void PlayState(string stateName) {
        anim.Play(stateName);
    }
}
