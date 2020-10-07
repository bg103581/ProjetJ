using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBonus : GeneratePrefabs
{
    [SerializeField]
    private ItemManager itemManager;

    public void SpawnBonus(Lane lane) {
        GameObject bonus = GetRandomPrefab();

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
        Instantiate(bonus, tr.position, tr.rotation, transform);
    }
}
