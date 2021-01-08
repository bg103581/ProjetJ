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
    private GameManager gameManager;
    private BonusSpawnRates bonusSpawnRates;

    [SerializeField]
    private float timePeriod;
    public Transform leftLane;
    public Transform centerLane;
    public Transform rightLane;
    public Transform topPos;
    public Transform vehiclePos;
    [Header("Chances in percentage")]
    [SerializeField]
    private int discsOnlyRate;
    [SerializeField]
    private int obstacleOnlyRate;
    [SerializeField]
    private int bonusRate;

    private int currentBonusRate = 0;

    private void Awake() {
        bonusSpawnRates = FindObjectOfType<BonusSpawnRates>();

        GameEvents.current.onReplayButtonTrigger += OnReplay;
        GameEvents.current.onMainMenuButtonTrigger += OnReplay;
    }

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        GenerateItems[] arr = FindObjectsOfType<GenerateItems>();
        foreach (GenerateItems generateItem in arr) {
            if (generateItem.itemType == ItemType.BONUS)
                generateBonuses = generateItem;
            else
                generateObstacles = generateItem;
        }
        //generateObstacles = FindObjectOfType<GenerateItems>();
        generateDiscs = FindObjectOfType<GenerateDiscs>();
    }

    private void OnDestroy() {
        GameEvents.current.onReplayButtonTrigger -= OnReplay;
        GameEvents.current.onMainMenuButtonTrigger -= OnReplay;
    }

    private void OnReplay() {
        CancelInvoke();
        currentBonusRate = 0;
    }

    public void StartSpawnItems() {
        InvokeRepeating("SpawnItem", 0f, timePeriod);
    }

    private void SpawnItem() {
        if (gameManager.gameState == GameState.PLAYING) {
            ItemType itemType = ChoseItem();
            Lane lane = GetRandomLane();

            switch (itemType) {
                case ItemType.OBSTACLE_DISCS:
                    GameObject obstacle = generateObstacles.SpawnItem(lane);
                    generateDiscs.SpawnDiscs(lane, obstacle);
                    break;
                case ItemType.DISCS_ONLY:
                    generateDiscs.SpawnDiscs(lane);
                    break;
                case ItemType.OBSTACLE_ONLY:
                    generateObstacles.SpawnItem(lane);
                    break;
                case ItemType.BONUS:
                    if (bonusSpawnRates.sumSpawnRates > 0) generateBonuses.SpawnItem(lane);
                    else generateObstacles.SpawnItem(lane);
                    break;
                default:
                    break;
            }
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
        else if (rand < obstacleOnlyRate + discsOnlyRate + currentBonusRate) {
            return ItemType.BONUS;
        }
        else {
            return ItemType.OBSTACLE_DISCS;
        }
    }

    public void StartSpawnOvniDiscs() {
        generateDiscs.SpawnOvniDiscs();
    }

    public void EnableBonus() {
        currentBonusRate = bonusRate;
    }
}
