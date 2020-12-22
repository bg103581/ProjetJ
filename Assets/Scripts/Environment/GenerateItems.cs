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
    private GameManager gameManager;

    private void Awake() {
        bonusSpawnRates = GetComponent<BonusSpawnRates>();
        gameManager = FindObjectOfType<GameManager>();
    }

    public GameObject SpawnItem(Lane lane) {
        GameObject item;

        if (itemType == ItemType.BONUS)
            item = GetRandomPrefab(bonusSpawnRates.spawnRates);
        else
            item = GetRandomPrefab();

        if (item == null) {
            return null;
        }

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

        GameObject go;
        if (itemType == ItemType.OBSTACLE_ONLY) {
            if (item.tag == "Voiture" || item.tag == "Camionette") {
                Vector3 pos = new Vector3(tr.position.x, tr.position.y, itemManager.vehiclePos.position.z);
                go = Instantiate(item, pos, tr.rotation, transform);
            }
            else {
                go = Instantiate(item, tr.position, tr.rotation, transform);
            }

            go.GetComponent<Obstacles>().currentLane = lane;
            return go;
        }
        else {
            go = Instantiate(item, tr.position, tr.rotation, transform);
            return go;
        }
    }
}
