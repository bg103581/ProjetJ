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
    
    public void StartSpawnLateralObjects() {
        InvokeRepeating("SpawnObj", spawnFreq, spawnFreq);
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

        Quaternion rot;
        if (isRight) {
            rot = Quaternion.Euler(new Vector3(spawnPos.rotation.x, spawnPos.rotation.y + 180, spawnPos.rotation.z));
        }
        else {
            rot = spawnPos.rotation;
        }

        GameObject lateralObj = Instantiate(GetRandomPrefab(), spawnPos.position, rot, transform);
        lateralObj.GetComponent<LateralMovement>().isStartingRight = isRight;
    }

    //private Transform GetRandomSpawnPos() {
    //    int rand = Random.Range(0, 2);

    //    if (rand == 0) return rightSpawnPosition;
    //    else return leftSpawnPosition;
    //}
}
