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

    [Header("Camera Move Time")]
    public float flyMoveTime;
    public float ovniMoveTime;
    public float goDownMoveTime;

    private Vector3 startPos;

    private void Awake() {
        startPos = transform.position;
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
        transform.DOMoveY(centerPos.position.y, goDownMoveTime).SetEase(Ease.OutSine);
    }

    public void MoveToOvniPosY() {
        transform.DOMoveY(ovniPos.position.y, ovniMoveTime).SetEase(Ease.OutSine);
    }

    public void MoveToOvniPosZ() {
        transform.DOMoveZ(ovniPos.position.z, ovniMoveTime).SetEase(Ease.OutSine);
    }

    public void MoveToNormalPosZ() {
        transform.DOMoveZ(startPos.z, goDownMoveTime).SetEase(Ease.OutSine);
    }
}
