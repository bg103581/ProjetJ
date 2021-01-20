using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(MoveItems))]
public class Obstacles : MonoBehaviour
{
    [HideInInspector]
    public Lane currentLane;

    private ItemManager itemManager;

    [SerializeField]
    private Collider[] colliders;
    [SerializeField]
    private float throwDuration;
    [SerializeField]
    private float throwRotationDuration;

    private void Awake() {
        itemManager = FindObjectOfType<ItemManager>();
    }

    public void Throw() {
        //disable moveitems/lateral movement + colliders
        GetComponent<MoveItems>().enabled = false;
        LateralMovement lateralMovement = GetComponent<LateralMovement>();
        if (lateralMovement != null) lateralMovement.enabled = false;
        foreach (Collider col in colliders) {
            col.enabled = false;
        }
        //sequence
        Sequence throwSequence = DOTween.Sequence();

        switch (currentLane) {
            case Lane.LEFT:
                throwSequence.Append(transform.DOMove(itemManager.leftThrowPos.position, throwDuration));
                throwSequence.Join(transform.DORotate(itemManager.leftThrowPos.rotation.eulerAngles, throwRotationDuration));
                break;
            case Lane.CENTER:
                int rand = Random.Range(0, 2);

                if (rand == 0) {
                    throwSequence.Append(transform.DOMove(itemManager.leftThrowPos.position, throwDuration));
                    throwSequence.Join(transform.DORotate(itemManager.leftThrowPos.rotation.eulerAngles, throwRotationDuration));
                }
                else {
                    throwSequence.Append(transform.DOMove(itemManager.rightThrowPos.position, throwDuration));
                    throwSequence.Join(transform.DORotate(itemManager.rightThrowPos.rotation.eulerAngles, throwRotationDuration));
                }
                break;
            case Lane.RIGHT:
                throwSequence.Append(transform.DOMove(itemManager.rightThrowPos.position, throwDuration));
                throwSequence.Join(transform.DORotate(itemManager.rightThrowPos.rotation.eulerAngles, throwRotationDuration));
                break;
            default:
                break;
        }
        throwSequence.SetEase(Ease.OutCubic);
        //destroy
        throwSequence.OnComplete(CallBackThrow);

        throwSequence.Play();
    }

    private void CallBackThrow() {
        Destroy(gameObject);
    }
}
