using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveItems : MonoBehaviour
{
    private Camera _mainCamera;
    private DiscMagnet discMagnet;
    private Player player;

    [HideInInspector]
    public bool isMovingToPlayer = false;

    [SerializeField]
    private float _moveSpeed;

    private void Start() {
        _mainCamera = FindObjectOfType<Camera>();
        discMagnet = FindObjectOfType<DiscMagnet>();
        player = FindObjectOfType<Player>();

        transform.position += Vector3.back * _moveSpeed * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMovingToPlayer)
            transform.DOMove(discMagnet.transform.position, discMagnet.moveDuration).SetEase(Ease.OutSine);
        else
            transform.position += Vector3.back * _moveSpeed * Time.deltaTime;

        if (_mainCamera.WorldToViewportPoint(transform.position).z < 0) {
            if (tag == "Alien") player.EndFly();
            Destroy(gameObject);
        }
    }

    //public void MoveToPlayer(Vector3 playerPos, float moveDuration) {
    //    if (!isMovingToPlayer) {
    //        isMovingToPlayer = true;

    //        transform.DOMove(playerPos, moveDuration);
    //    }
    //}
}
