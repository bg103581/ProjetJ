using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateObstacles : GeneratePrefabs
{
    [SerializeField]
    private ItemManager itemManager;
    //scriptable object pour les obstacles ? ils ont chacun leur vitesse

    //every timePeriod seconds spawn a random obstacle on a random lane

    private void Start() {
        //InvokeRepeating("SpawnObstacle", 0f, _timePeriod);  //peut etre a modifier pour que ça soit pas tout le temps
        //toutes les x secondes
        
    }

    public void SpawnObstacle(Lane lane) {
        GameObject obstacle = GetRandomPrefab();

        Transform tr;
        switch (lane) {
            case Lane.LEFT:
                tr = itemManager.leftLane;
                break;
            case Lane.CENTER:
                tr = itemManager.centerLane;
                break;
            case Lane.RIGHT:
                tr = itemManager.rightLane;
                break;
            default:
                tr = itemManager.leftLane;
                break;
        }
        Instantiate(obstacle, tr.position, tr.rotation, transform);
    }
}
