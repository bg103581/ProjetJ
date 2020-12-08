using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CopStartAnimMovement : MonoBehaviour
{
    private Cop cop;

    [SerializeField]
    private float startWalkTime;

    private void Awake() {
        cop = FindObjectOfType<Cop>();
    }

    public void PlayStartMovement() {
        Sequence copSequence = DOTween.Sequence();

        copSequence.Append(transform.DOMove(cop.copCatchUpPos.position, startWalkTime).SetEase(Ease.Linear));
        copSequence.AppendCallback(() => cop.TriggerSuprisedAnimation());

        copSequence.Play();
        //transform.DOMove(cop.copCatchUpPos.position, startWalkTime).SetEase(Ease.Linear);
    }
}
