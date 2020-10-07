using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { OBSTACLE_DISCS, DISCS_ONLY, OBSTACLE_ONLY, BONUS }
public enum Lane { LEFT, CENTER, RIGHT }

public class ItemManager : MonoBehaviour
{
    private GenerateItems generateObstacles;
    private GenerateItems generateBonuses;
    private GenerateDiscs generateDiscs;

    [SerializeField]
    private float timePeriod;
    public Transform leftLane;
    public Transform centerLane;
    public Transform rightLane;
    [Header("Chances in percentage")]
    [SerializeField]
    private int discsOnlyRate;
    [SerializeField]
    private int obstacleOnlyRate;
    [SerializeField]
    private int bonusRate;

    // Start is called before the first frame update
    void Start()
    {
        GenerateItems[] arr = FindObjectsOfType<GenerateItems>();
        foreach (GenerateItems generateItem in arr) {
            if (generateItem.itemType == ItemType.BONUS)
                generateBonuses = generateItem;
            else
                generateObstacles = generateItem;
        }
        //generateObstacles = FindObjectOfType<GenerateItems>();
        generateDiscs = FindObjectOfType<GenerateDiscs>();

        InvokeRepeating("SpawnItem", 0f, timePeriod);
    }

    private void SpawnItem() {
        ItemType itemType = ChoseItem();
        Lane lane = GetRandomLane();

        switch (itemType) {
            case ItemType.OBSTACLE_DISCS:
                generateObstacles.SpawnItem(lane);
                generateDiscs.SpawnDiscs(lane, itemType);
                break;
            case ItemType.DISCS_ONLY:
                generateDiscs.SpawnDiscs(lane, itemType);
                break;
            case ItemType.OBSTACLE_ONLY:
                generateObstacles.SpawnItem(lane);
                break;
            case ItemType.BONUS:
                generateBonuses.SpawnItem(lane);
                break;
            default:
                break;
        }
    }

    private Lane GetRandomLane() {
        int rand = Random.Range(0, 3);

        if (rand == 0) return Lane.LEFT;
        else if (rand == 1) return Lane.CENTER;
        else return Lane.RIGHT;
    }

    private ItemType ChoseItem() {
        int rand = Random.Range(0, 101);

        if (rand < obstacleOnlyRate) {
            return ItemType.OBSTACLE_ONLY;
        }
        else if (rand < obstacleOnlyRate + discsOnlyRate) {
            return ItemType.DISCS_ONLY;
        }
        else if (rand < obstacleOnlyRate + discsOnlyRate + bonusRate) {
            return ItemType.BONUS;
        }
        else {
            return ItemType.OBSTACLE_DISCS;
        }
    }
}
