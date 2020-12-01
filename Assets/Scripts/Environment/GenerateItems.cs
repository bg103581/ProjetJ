using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateItems : GeneratePrefabs
{
    [SerializeField]
    private ItemManager itemManager;
    //scriptable object pour les obstacles ? ils ont chacun leur vitesse
    
    public ItemType itemType;
    
    private BonusSpawnRates bonusSpawnRates;

    private void Awake() {
        bonusSpawnRates = GetComponent<BonusSpawnRates>();
    }

    public void SpawnItem(Lane lane) {
        GameObject item;

        if (itemType == ItemType.BONUS)
            item = GetRandomPrefab(bonusSpawnRates.spawnRates);
        else
            item = GetRandomPrefab();

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

        if (item.tag == "Voiture" || item.tag == "Camionette") {
            Vector3 pos = new Vector3(tr.position.x, tr.position.y, itemManager.vehiclePos.position.z);
            Instantiate(item, pos, tr.rotation, transform);
        }
        else {
            Instantiate(item, tr.position, tr.rotation, transform);
        }
    }
}
