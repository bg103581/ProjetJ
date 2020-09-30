using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveItems : MonoBehaviour
{

    private Camera _mainCamera;
    [SerializeField]
    private float _moveSpeed;

    private void Start() {
        _mainCamera = FindObjectOfType<Camera>();

        transform.position += Vector3.back * _moveSpeed * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.back * _moveSpeed * Time.deltaTime;

        if (_mainCamera.WorldToViewportPoint(transform.position).z < 0) {
            Destroy(gameObject);
        }
    }
}
