using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DiscMagnet : MonoBehaviour
{
    private Player player;
    private Collider[] discsAttracted;

    public float moveDuration;
    [SerializeField]
    private float magnetRadius;
    [SerializeField]
    private float yOffSet;

    private Vector3 magnetSize;

    private void Awake() {
        player = transform.parent.GetComponent<Player>();
        magnetSize = new Vector3(magnetRadius, magnetRadius, 2f);
    }

    private void Update() {
        if (player.isPochon) {
            discsAttracted = Physics.OverlapBox(transform.position + Vector3.up * yOffSet, magnetSize, Quaternion.identity, 1 << 10);

            foreach (Collider disc in discsAttracted) {
                disc.transform.GetComponent<MoveItems>().isMovingToPlayer = true;
            }
        }
    }

    //private void OnTriggerEnter(Collider other) {
    //    if (other.gameObject.layer == 10) {
    //        //disable move item script
    //        //other.gameObject.GetComponent<MoveItems>().enabled = false;

    //        //attract disc to player
    //        //AttractDisc(other.transform);
    //    }
    //}

    //private void AttractDisc(Transform disc) {
    //    disc.parent.GetComponent<MoveItems>().MoveToPlayer(transform.position, moveDuration);
    //}

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, magnetRadius);
        Gizmos.DrawWireCube(transform.position + Vector3.up * yOffSet, magnetSize * 2f);
    }
}
