using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private Transform leftPos;
    [SerializeField]
    private Transform centerPos;
    [SerializeField]
    private Transform rightPos;
    [SerializeField]
    private Transform topPos;
    [SerializeField]
    private Transform ovniPos;
    [SerializeField]
    private Transform playPos;

    [Header("Camera Move Time")]
    public float flyMoveTime;
    public float ovniMoveTime;
    public float goDownMoveTime;
    public float playMoveTime;

    private Vector3 startPos;
    private Quaternion startRot;
    private GameManager gameManager;

    private void Awake() {
        startPos = transform.position;
        startRot = transform.rotation;

        gameManager = FindObjectOfType<GameManager>();

        GameEvents.current.onReplayButtonTrigger += OnReplay;
        GameEvents.current.onMainMenuButtonTrigger += OnReplay;
    }

    private void OnDestroy() {
        GameEvents.current.onReplayButtonTrigger -= OnReplay;
        GameEvents.current.onMainMenuButtonTrigger -= OnReplay;
    }

    private void OnReplay() {
        transform.position = startPos;
        transform.rotation = startRot;
    }

    public void MoveToLeftPos(float moveTime) {
        transform.DOMoveX(leftPos.position.x, moveTime).SetEase(Ease.OutSine);
    }

    public void MoveToCenterPos(float moveTime) {
        transform.DOMoveX(centerPos.position.x, moveTime).SetEase(Ease.OutSine);
    }

    public void MoveToRightPos(float moveTime) {
        transform.DOMoveX(rightPos.position.x, moveTime).SetEase(Ease.OutSine);
    }

    public void MoveToTopPos() {
        transform.DOMoveY(topPos.position.y, flyMoveTime).SetEase(Ease.OutSine);
    }

    public void MoveToGroundPos() {
        transform.DOMoveY(playPos.position.y, goDownMoveTime).SetEase(Ease.OutSine);
    }

    public void MoveToOvniPosY() {
        transform.DOMoveY(ovniPos.position.y, ovniMoveTime).SetEase(Ease.OutSine);
    }

    public void MoveToOvniPosZ() {
        transform.DOMoveZ(ovniPos.position.z, ovniMoveTime).SetEase(Ease.OutSine);
    }

    public void MoveToNormalPosZ() {
        transform.DOMoveZ(playPos.position.z, goDownMoveTime).SetEase(Ease.OutSine);
    }

    public void MoveToPlayPos() {
        Sequence camSequence = DOTween.Sequence();

        camSequence.Append(transform.DOMove(playPos.position, playMoveTime).SetEase(Ease.OutSine));
        camSequence.Join(transform.DORotate(playPos.rotation.eulerAngles, playMoveTime).SetEase(Ease.OutSine));
        camSequence.AppendCallback(() => gameManager.StartPlayingInputs());

        camSequence.Play();

        //transform.DOMove(playPos.position, playMoveTime).SetEase(Ease.OutSine);
        //transform.DORotate(playPos.rotation.eulerAngles, playMoveTime).SetEase(Ease.OutSine);
    }
}
