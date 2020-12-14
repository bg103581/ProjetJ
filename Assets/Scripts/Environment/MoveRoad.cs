﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRoad : MonoBehaviour
{
    private GameManager gameManager;
    private GenerateRoads generateRoads;
    private float speed;
    private Camera mainCamera;

    public Transform startPos;
    public Transform endPos;

    // Start is called before the first frame update
    void Start()
    {
        generateRoads = GetComponentInParent<GenerateRoads>();
        speed = generateRoads.roadSpeed;
        mainCamera = FindObjectOfType<Camera>();
        gameManager = FindObjectOfType<GameManager>();

        GameEvents.current.onReplayButtonTrigger += OnReplay;

        if (gameManager.gameState == GameState.PLAYING)
            transform.position += Vector3.back * speed * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.gameState == GameState.PLAYING)
            transform.position += Vector3.back * speed * Time.deltaTime;

        if ( mainCamera.WorldToViewportPoint(endPos.position).z < 0 ) {
            generateRoads.CreateRoad();
            Destroy(gameObject);
        }
    }

    private void OnDestroy() {
        GameEvents.current.onReplayButtonTrigger -= OnReplay;
    }

    private void OnReplay() {
        Destroy(gameObject);
    }
}
