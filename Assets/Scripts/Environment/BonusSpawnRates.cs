using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusSpawnRates : MonoBehaviour
{
    public int[] spawnRates;

    public void ChangeSpawnRates(int index, int value) {
        if (index >= spawnRates.Length || index < 0) {
            Debug.Log("Index of Spawn Rates is incorrect");
            return;
        }

        spawnRates[index] = value;
    }

    public void ChangeClaquetteSpawnRate(int value) {
        ChangeSpawnRates(0, value);
    }

    public void ChangePochonSpawnRate(int value) {
        ChangeSpawnRates(1, value);
    }

    public void ChangeTwingoSpawnRate(int value) {
        ChangeSpawnRates(2, value);
    }

    public void ChangeTmaxSpawnRate(int value) {
        ChangeSpawnRates(3, value);
    }
}
