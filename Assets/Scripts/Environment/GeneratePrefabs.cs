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

}
