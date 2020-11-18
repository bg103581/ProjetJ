using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjRandPos : GeneratePrefabs
{
    [SerializeField]
    private Transform rightSpawnPosition;
    [SerializeField]
    private Transform leftSpawnPosition;
    [SerializeField]
    private float spawnFreq;
    
    void Start()
    {
        InvokeRepeating("SpawnObj", 10f, spawnFreq);
    }

    private void SpawnObj() {
        Transform spawnPos;
        bool isRight;
        int rand = Random.Range(0, 2);

        if (rand == 0) {
            spawnPos = rightSpawnPosition;
            isRight = true;
        }
        else {
            spawnPos = leftSpawnPosition;
            isRight = false;
        }

        GameObject lateralObj = Instantiate(GetRandomPrefab(), spawnPos.position, spawnPos.rotation, transform);
        lateralObj.GetComponent<LateralMovement>().isStartingRight = isRight;
    }

    //private Transform GetRandomSpawnPos() {
    //    int rand = Random.Range(0, 2);

    //    if (rand == 0) return rightSpawnPosition;
    //    else return leftSpawnPosition;
    //}
}
