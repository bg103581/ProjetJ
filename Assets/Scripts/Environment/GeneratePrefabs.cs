using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePrefabs : MonoBehaviour
{

    [SerializeField]
    protected GameObject[] _prefabs;

    protected GameObject GetRandomPrefab() {
        if (_prefabs.Length == 0) {
            return null;
        }
        int rand = Random.Range(0, _prefabs.Length);
        return _prefabs[rand];
    }

    protected GameObject GetRandomPrefab(int[] spawnRates) {    //spawnRates must be the same length as _prefabs and the summ should be 100
        if (_prefabs.Length == 0) {
            return null;
        }

        //sum of spawnrates = max range random
        int maxRange = Sum(spawnRates) + 1;
        int rand = Random.Range(1, maxRange);
        int rateCount = 0;
        for (int i = 0; i < spawnRates.Length; i++) {
            rateCount += spawnRates[i];
            if (rand <= rateCount)
                return _prefabs[i];
        }
        
        return null;
    }

    private int Sum(int[] spawnRates) {
        int sum = 0;
        for (int i = 0; i < spawnRates.Length; i++) {
            sum += spawnRates[i];
        }

        return sum;
    }
}
