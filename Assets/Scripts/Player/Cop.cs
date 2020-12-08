using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Cop : MonoBehaviour
{
    
    private bool isFollowingPlayer;
    private CopStartAnimMovement copStartAnimMovement;

    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Transform initialPos;
    public Transform copCatchUpPos;
    [SerializeField]
    private float catchUpMoveTime;
    [SerializeField]
    private float backToInitMoveTime;

    private void Awake() {
        copStartAnimMovement = GetComponent<CopStartAnimMovement>();
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
        transform.DOMoveZ(initialPos.position.z, backToInitMoveTime);
    }

    public void Jump() {
        anim.SetTrigger("jumpTrigger");
    }

    public void Strafe() {
        anim.SetTrigger("strafeTrigger");
    }

    public void StartAnimation() {
        anim.SetTrigger("startAnimationTrigger");

        copStartAnimMovement.PlayStartMovement();
    }

    public void TriggerSuprisedAnimation() {
        anim.SetTrigger("surprisedTrigger");
    }
}
