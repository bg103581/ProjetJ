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
    private Transform camTransform = null;

    private void Awake() {
        camTransform = transform;
        startPos = camTransform.position;
        startRot = camTransform.rotation;

        gameManager = FindObjectOfType<GameManager>();

        GameEvents.current.onReplayButtonTrigger += OnReplay;
        GameEvents.current.onMainMenuButtonTrigger += OnReplay;
    }

    private void OnDestroy() {
        GameEvents.current.onReplayButtonTrigger -= OnReplay;
        GameEvents.current.onMainMenuButtonTrigger -= OnReplay;
    }

    private void OnReplay() {
        camTransform.position = startPos;
        camTransform.rotation = startRot;
    }

    public void MoveToLeftPos(float moveTime) {
        camTransform.DOMoveX(leftPos.position.x, moveTime).SetEase(Ease.OutSine);
    }

    public void MoveToCenterPos(float moveTime) {
        camTransform.DOMoveX(centerPos.position.x, moveTime).SetEase(Ease.OutSine);
    }

    public void MoveToRightPos(float moveTime) {
        camTransform.DOMoveX(rightPos.position.x, moveTime).SetEase(Ease.OutSine);
    }

    public void MoveToTopPos() {
        camTransform.DOMoveY(topPos.position.y, flyMoveTime).SetEase(Ease.OutSine);
    }

    public void MoveToGroundPos() {
        camTransform.DOMoveY(playPos.position.y, goDownMoveTime).SetEase(Ease.OutSine);
    }

    public void MoveToOvniPosY() {
        camTransform.DOMoveY(ovniPos.position.y, ovniMoveTime).SetEase(Ease.OutSine);
    }

    public void MoveToOvniPosZ() {
        camTransform.DOMoveZ(ovniPos.position.z, ovniMoveTime).SetEase(Ease.OutSine);
    }

    public void MoveToNormalPosZ() {
        camTransform.DOMoveZ(playPos.position.z, goDownMoveTime).SetEase(Ease.OutSine);
    }

    public void MoveToPlayPos() {
        Sequence camSequence = DOTween.Sequence();

        camSequence.Append(camTransform.DOMove(playPos.position, playMoveTime).SetEase(Ease.OutSine));
        camSequence.Join(camTransform.DORotate(playPos.rotation.eulerAngles, playMoveTime).SetEase(Ease.OutSine));
        camSequence.AppendCallback(() => gameManager.StartPlayingInputs());

        camSequence.Play();

        //transform.DOMove(playPos.position, playMoveTime).SetEase(Ease.OutSine);
        //transform.DORotate(playPos.rotation.eulerAngles, playMoveTime).SetEase(Ease.OutSine);
    }
}
