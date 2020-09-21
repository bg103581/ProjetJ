using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRoad : MonoBehaviour
{
    private GenerateRoads _generateRoads;
    private float _speed;
    private Camera _mainCamera;

    public Transform startPos;
    public Transform endPos;

    // Start is called before the first frame update
    void Start()
    {
        _generateRoads = GetComponentInParent<GenerateRoads>();
        _speed = _generateRoads.roadSpeed;
        _mainCamera = FindObjectOfType<Camera>();

        transform.position += Vector3.back * _speed * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.back * _speed * Time.deltaTime;

        if ( _mainCamera.WorldToViewportPoint(endPos.position).z < 0 ) {
            _generateRoads.CreateRoad();
            Destroy(gameObject);
        }
    }
}
