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
}
