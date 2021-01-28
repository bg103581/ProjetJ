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
    public Transform leftThrowPos;
    public Transform rightThrowPos;
    [Header("Chances in percentage")]
    [Header("start rates :")]
    [SerializeField]
    private int discsOnlyRate1;
    [SerializeField]
    private int obstacleOnlyRate1;
    [SerializeField]
    private int bonusRate1;
    [Header("medium rates :")]
    [SerializeField]
    private int discsOnlyRate2;
    [SerializeField]
    private int obstacleOnlyRate2;
    [SerializeField]
    private int bonusRate2;
    [Header("last rates :")]
    [SerializeField]
    private int discsOnlyRate3;
    [SerializeField]
    private int obstacleOnlyRate3;
    [SerializeField]
    private int bonusRate3;
    
    private int currentDiscsOnlyRate;
    private int currentObstacleOnlyRate;
    private int currentBonusRate = 0;

    [Header("Chances of 2 items")]
    public int twoObstaclesRate;
    public int twoDiscsRate;

    private void Awake() {
        bonusSpawnRates = FindObjectOfType<BonusSpawnRates>();

        currentDiscsOnlyRate = discsOnlyRate1;
        currentObstacleOnlyRate = obstacleOnlyRate1;

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
                    GameObject pattern = generateDiscs.SpawnDiscs(lane, obstacle);

                    int rand = Random.Range(0, 100);

                    if (rand < twoObstaclesRate) {
                        //spawn two obstacles
                        switch (lane) {
                            case Lane.LEFT:
                                if (pattern.tag == "Right") {
                                    GameObject secondObstacle = generateObstacles.SpawnItem(Lane.RIGHT);
                                    Lane[] availableLanes = { Lane.RIGHT };
                                    generateDiscs.SpawnSecondDiscs(secondObstacle, availableLanes);
                                }
                                else if (pattern.tag == "Jump") {

                                }
                                break;
                            case Lane.CENTER:
                                break;
                            case Lane.RIGHT:
                                break;
                            default:
                                break;
                        }
                    }
                    else {
                        //spawn one
                    }
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

        if (rand < currentObstacleOnlyRate) {
            return ItemType.OBSTACLE_ONLY;
        }
        else if (rand < currentObstacleOnlyRate + currentDiscsOnlyRate) {
            return ItemType.DISCS_ONLY;
        }
        else if (rand < currentObstacleOnlyRate + currentDiscsOnlyRate + currentBonusRate) {
            return ItemType.BONUS;
        }
        else {
            return ItemType.OBSTACLE_DISCS;
        }
    }

    public void ChangeItemRates(int step) {
        switch (step) {
            case 2:
                currentObstacleOnlyRate = obstacleOnlyRate2;
                currentDiscsOnlyRate = discsOnlyRate2;
                currentBonusRate = bonusRate2;
                break;
            case 3:
                currentObstacleOnlyRate = obstacleOnlyRate3;
                currentDiscsOnlyRate = discsOnlyRate3;
                currentBonusRate = bonusRate3;
                break;
            default:
                Debug.LogError("wrong step id");
                break;
        }
    }

    public void StartSpawnOvniDiscs() {
        generateDiscs.SpawnOvniDiscs();
    }

    public void EnableBonus() {
        currentBonusRate = bonusRate1;
    }
}
