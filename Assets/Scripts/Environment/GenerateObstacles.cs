using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateObstacles : GeneratePrefabs
{

    //scriptable object pour les obstacles ? ils ont chacun leur vitesse

    [SerializeField]
    private Transform _leftLane;
    [SerializeField]
    private Transform _centerLane;
    [SerializeField]
    private Transform _rightLane;
    [SerializeField]
    private float _timePeriod;

    //every timePeriod seconds spawn a random obstacle on a random lane

    private void Start() {
        InvokeRepeating("SpawnObstacle", 0f, _timePeriod);  //peut etre a modifier pour que ça soit pas tout le temps
                                                            //toutes les x secondes
    }

    private void SpawnObstacle() {
        GameObject obstacle = GetRandomPrefab();
        Transform pos = GetRandomLane();
        Instantiate(obstacle, pos.position, pos.rotation, transform);
    }

    private Transform GetRandomLane() {
        int rand = Random.Range(0, 3);

        if (rand == 0) return _leftLane;
        else if (rand == 1) return _centerLane;
        else return _rightLane;
    }
}
