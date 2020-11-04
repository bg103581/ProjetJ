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

    public void MoveToTopPos(float moveTime) {
        transform.DOMoveY(topPos.position.y, moveTime).SetEase(Ease.OutSine);
    }

    public void MoveToGroundPos(float moveTime) {
        transform.DOMoveY(centerPos.position.y, moveTime).SetEase(Ease.OutSine);
    }

    public void MoveToOvniPosY(float moveTime) {
        transform.DOMoveY(ovniPos.position.y, moveTime).SetEase(Ease.OutSine);
    }

    public void MoveToOvniPosZ(float moveTime) {
        transform.DOMoveZ(ovniPos.position.z, moveTime).SetEase(Ease.OutSine);
    }

    public void MoveToNormalPosZ(float moveTime) {
        transform.DOMoveZ(startPos.z, moveTime).SetEase(Ease.OutSine);
    }
}
