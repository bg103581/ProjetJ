using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveItems : MonoBehaviour
{
    private Camera _mainCamera;
    private DiscMagnet discMagnet;
    private Player player;
    private GameManager gameManager;
    private Transform itemTransform = null;

    [HideInInspector]
    public bool isMovingToPlayer = false;

    [SerializeField]
    private float _moveSpeed;

    private void Awake()
    {
        itemTransform = transform;
    }

    private void OnEnable() {
        GameEvents.current.onReplayButtonTrigger += OnReplay;
        GameEvents.current.onMainMenuButtonTrigger += OnReplay;
    }

    private void Start() {
        _mainCamera = FindObjectOfType<Camera>();
        discMagnet = FindObjectOfType<DiscMagnet>();
        player = FindObjectOfType<Player>();
        gameManager = FindObjectOfType<GameManager>();

        if (gameManager.gameState == GameState.PLAYING)
            itemTransform.position += Vector3.back * _moveSpeed * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.gameState == GameState.PLAYING) {
            if (isMovingToPlayer)
                itemTransform.DOMove(discMagnet.transform.position, discMagnet.moveDuration).SetEase(Ease.OutSine);
            else
                itemTransform.position += Vector3.back * _moveSpeed * Time.deltaTime;
        }

        if (_mainCamera.WorldToViewportPoint(itemTransform.position).z < 0) {
            if (tag == "Alien") player.EndFly();
            ObjectPooler.current.DestroyObject(gameObject);
            //Destroy(gameObject);
        }
    }

    private void OnDestroy() {
        GameEvents.current.onReplayButtonTrigger -= OnReplay;
        GameEvents.current.onMainMenuButtonTrigger -= OnReplay;

        itemTransform.DOKill();
    }

    private void OnDisable() {
        GameEvents.current.onReplayButtonTrigger -= OnReplay;
        GameEvents.current.onMainMenuButtonTrigger -= OnReplay;

        isMovingToPlayer = false;
    }

    private void OnReplay() {
        ObjectPooler.current.DestroyObject(gameObject);
        //Destroy(gameObject);
    }
    

    //public void MoveToPlayer(Vector3 playerPos, float moveDuration) {
    //    if (!isMovingToPlayer) {
    //        isMovingToPlayer = true;

    //        transform.DOMove(playerPos, moveDuration);
    //    }
    //}
}
