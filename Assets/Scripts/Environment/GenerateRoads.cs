using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateRoads : GeneratePrefabs
{

    [SerializeField]
    private GameObject startingRoad;
    [SerializeField]
    [Range(3, 10)]
    private int _nbRoadToInit;
    [SerializeField]
    private Transform _initPosInstantiate;

    private GameObject[] _roadsInGame;
    private MoveRoad moveRoad;
    private MoveRoad previousMoveRoad;

    public float roadSpeed;

    private void Awake() {
        GameEvents.current.onReplayButtonTrigger += OnReplay;
    }

    private void Start() {
        InitRoad();
    }

    private void OnDestroy() {
        GameEvents.current.onReplayButtonTrigger -= OnReplay;
    }

    private void OnReplay() {
        InitRoad();
    }

    private void InitRoad() {
        _roadsInGame = new GameObject[_nbRoadToInit];
        for (int i = 0; i < _nbRoadToInit; i++) {
            _roadsInGame[i] = GetRandomPrefab();
        }

        //instantiate la starting road
        _roadsInGame[0] = Instantiate(startingRoad, _initPosInstantiate.position, _initPosInstantiate.rotation, transform);

        //instantiate le reste en fonction du précedent
        for (int i = 1; i < _roadsInGame.Length; i++) {
            previousMoveRoad = _roadsInGame[i - 1].GetComponent<MoveRoad>();
            moveRoad = _roadsInGame[i].GetComponent<MoveRoad>();
            _roadsInGame[i] = Instantiate(
                _roadsInGame[i],
                previousMoveRoad.endPos.position + (_roadsInGame[i].transform.position - moveRoad.startPos.position),
                _initPosInstantiate.rotation,
                transform);
        }
    }

    public void CreateRoad() {
        GameObject lastRoad;

        //update le tableau _roadsInGame
        _roadsInGame[0] = null;
        for (int i = 1; i < _roadsInGame.Length; i++) {
            _roadsInGame[i - 1] = _roadsInGame[i];
        }
        _roadsInGame[_roadsInGame.Length - 1] = GetRandomPrefab();
        lastRoad = _roadsInGame[_roadsInGame.Length - 1];

        moveRoad = lastRoad.GetComponent<MoveRoad>();
        previousMoveRoad = _roadsInGame[_roadsInGame.Length - 2].GetComponent<MoveRoad>();
        _roadsInGame[_roadsInGame.Length - 1] = Instantiate(
                 lastRoad,
                 previousMoveRoad.endPos.position + (lastRoad.transform.position - moveRoad.startPos.position),
                 _initPosInstantiate.rotation,
                 transform);
    }
}
