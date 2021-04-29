using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRoad : MonoBehaviour
{
    private GameManager gameManager;
    private GenerateRoads generateRoads;
    private float speed;
    private Camera mainCamera;
    private Transform roadTransform = null;

    public Transform startPos;
    public Transform endPos;

    private void Awake()
    {
        roadTransform = transform;
    }

    void Start()
    {
        generateRoads = GetComponentInParent<GenerateRoads>();
        speed = generateRoads.roadSpeed;
        mainCamera = FindObjectOfType<Camera>();
        gameManager = FindObjectOfType<GameManager>();

        GameEvents.current.onReplayButtonTrigger += OnReplay;
        GameEvents.current.onMainMenuButtonTrigger += OnReplay;

        if (gameManager.gameState == GameState.PLAYING)
            roadTransform.position += Vector3.back * speed * Time.deltaTime;
    }
    
    void Update()
    {
        if (gameManager.gameState == GameState.PLAYING)
            roadTransform.position += Vector3.back * speed * Time.deltaTime;

        if ( mainCamera.WorldToViewportPoint(endPos.position).z < 0 ) {
            generateRoads.CreateRoad();
            Destroy(gameObject);
        }
    }

    private void OnDestroy() {
        GameEvents.current.onReplayButtonTrigger -= OnReplay;
        GameEvents.current.onMainMenuButtonTrigger -= OnReplay;
    }

    private void OnReplay() {
        Destroy(gameObject);
    }
}
