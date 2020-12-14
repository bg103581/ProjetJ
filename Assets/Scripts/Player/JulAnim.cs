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

    private Vector3 startPos;
    private Quaternion startRot;

    private Animator anim;
    private GameManager gameManager;

    private void Awake() {
        startPos = transform.localPosition;
        startRot = transform.localRotation;
        anim = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();

        GameEvents.current.onReplayButtonTrigger += OnReplay;
        GameEvents.current.onMainMenuButtonTrigger += OnReplay;
    }
    
    private void OnDestroy() {
        GameEvents.current.onReplayButtonTrigger -= OnReplay;
        GameEvents.current.onMainMenuButtonTrigger -= OnReplay;
    }

    private void OnEnable() {
        if (gameManager.gameState == GameState.PLAYING)
         anim.Play("Run");
    }

    private void OnReplay() {
        transform.localPosition = startPos;
        transform.localRotation = startRot;
    }

    public void PlayStartMovement() {
        Sequence mySequence = DOTween.Sequence();

        mySequence.Append(transform.DOMoveX(0f, moveXTime).SetEase(Ease.OutSine));
        mySequence.Join(transform.DORotate(new Vector3(0f, 0f, 0f), rotateRunTime).SetEase(Ease.OutSine));

        mySequence.Play();
    }

    public void RotatePlayPos() {
        Debug.Log("rotate to play");
        transform.DOLocalRotate(new Vector3(0f, -1f ,0f) , rotatePlayPosTime);
    }

    public void Strafe() {
        anim.SetTrigger("strafeTrigger");
    }

    public void Jump() {
        anim.SetTrigger("jumpTrigger");
    }

    public void StartAnimation() {
        anim.SetTrigger("startAnimationTrigger");
    }
}
