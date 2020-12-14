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

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        Move();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move() {
        if (gameManager.gameState == GameState.PLAYING) {
            if (isStartingRight) {
                transform.position += Vector3.left * lateralMoveSpeed * Time.deltaTime;
            }
            else {
                transform.position += Vector3.right * lateralMoveSpeed * Time.deltaTime;
            }
        }
    }
}
