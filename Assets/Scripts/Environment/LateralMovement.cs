using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LateralMovement : MonoBehaviour
{
    [HideInInspector]
    public bool isStartingRight = true;
    [SerializeField]
    private float lateralMoveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        Move();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move() {
        if (isStartingRight) {
            transform.position += Vector3.left * lateralMoveSpeed * Time.deltaTime;
        }
        else {
            transform.position += Vector3.right * lateralMoveSpeed * Time.deltaTime;
        }
    }
}
