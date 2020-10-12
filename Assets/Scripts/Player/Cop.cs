using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Cop : MonoBehaviour
{
    private Vector3 initialPos;
    private bool isFollowingPlayer;

    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Transform copCatchUpPos;
    [SerializeField]
    private float catchUpMoveTime;
    [SerializeField]
    private float backToInitMoveTime;

    private void Awake() {
        initialPos = transform.position;
    }

    private void Update() {
        if (isFollowingPlayer) {
            transform.DOMove(copCatchUpPos.position, catchUpMoveTime);
        }
    }

    public void CatchUpToPlayer() {
        isFollowingPlayer = true;
    }

    public void GoBackToInitialPos() {
        isFollowingPlayer = false;
        transform.DOMoveZ(initialPos.z, backToInitMoveTime);
    }

    public void Jump() {
        anim.SetTrigger("jumpTrigger");
    }

    public void Strafe() {
        anim.SetTrigger("strafeTrigger");
    }
}
