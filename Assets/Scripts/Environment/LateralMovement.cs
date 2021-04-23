using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LateralMovement : MonoBehaviour
{
    [HideInInspector]
    public bool isStartingRight = true;
    [SerializeField]
    private float lateralMoveSpeed;

    private GameManager gameManager;
    private Transform _transform = null;

    private void Awake()
    {
        _transform = transform;
    }

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        Move();
    }
    
    void Update()
    {
        Move();
    }

    private void Move() {
        if (gameManager.gameState == GameState.PLAYING) {
            if (isStartingRight) {
                _transform.position += Vector3.left * lateralMoveSpeed * Time.deltaTime;
            }
            else {
                _transform.position += Vector3.right * lateralMoveSpeed * Time.deltaTime;
            }
        }
    }
}
