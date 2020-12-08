using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class JulStartAnimMovement : MonoBehaviour
{

    [SerializeField]
    private float moveXTime = 0.5f;
    [SerializeField]
    private float rotateRunTime = 0.5f;
    [SerializeField]
    private float rotatePlayPosTime = 0.5f;

    public void PlayStartMovement() {
        Debug.Log("move x");
        Sequence mySequence = DOTween.Sequence();

        mySequence.Append(transform.DOMoveX(0f, moveXTime).SetEase(Ease.OutSine));
        mySequence.Join(transform.DORotate(new Vector3(0f, 0f, 0f), rotateRunTime).SetEase(Ease.OutSine));

        mySequence.Play();
    }

    public void RotatePlayPos() {
        Debug.Log("rotate to play");
        transform.DOLocalRotate(new Vector3(0f, -1f ,0f) , rotatePlayPosTime);
    }
}
