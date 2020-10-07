using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateItems : GeneratePrefabs
{
    [SerializeField]
    private ItemManager itemManager;
    //scriptable object pour les obstacles ? ils ont chacun leur vitesse
    
    public ItemType itemType;

    public void SpawnItem(Lane lane) {
        GameObject item = GetRandomPrefab();

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
        Instantiate(item, tr.position, tr.rotation, transform);
    }
}
