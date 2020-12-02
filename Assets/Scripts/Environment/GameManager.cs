using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Reflection;

public enum GameState { WAITING, ANIMATION_START, PLAYING, FINISHED }

public class GameManager : MonoBehaviour
{
    #region Variables
    private const float COEF_UNIT_METER = 0.583f;
    
    private GenerateRoads generateRoads;
    private Player player;
    private ItemManager itemManager;
    private BonusSpawnRates bonusSpawnRates;
    private SpawnObjRandPos spawnObjRandPos;

    private int dodgeMultiplier = 1;
    private int ovniMultiplier = 2;

    [SerializeField]
    private float score = 0;
    [SerializeField]
    private int nbGoldDiscs = 0;
    [SerializeField]
    private int nbPlatDiscs = 0;
    [SerializeField]
    private float traveledDistance = 0;
    [SerializeField]
    private float distanceUpgradeDifficulty;
    [SerializeField]
    private float timescaleDifficulty;

    [Header("Steps of bonus spawn and their spawn rates")]
    [SerializeField]
    private float distanceStartSpawningPochonClaquette;
    [SerializeField]
    private int pochonSpawnRate_1;
    [SerializeField]
    private int claquetteSpawnRate_1;
    [SerializeField]
    private float distanceStartSpawningTwingo;
    [SerializeField]
    private int pochonSpawnRate_2;
    [SerializeField]
    private int claquetteSpawnRate_2;
    [SerializeField]
    private int TwingoSpawnRate_2;
    [SerializeField]
    private float distanceStartSpawningTmax;
    [SerializeField]
    private int pochonSpawnRate_3;
    [SerializeField]
    private int claquetteSpawnRate_3;
    [SerializeField]
    private int TwingoSpawnRate_3;
    [SerializeField]
    private int TmaxSpawnRate_3;

    private float upgradeDifficultyHolder;

    [HideInInspector]
    public GameState gameState = GameState.WAITING;
    
    #endregion

    #region MonoBehaviour
    private void Start() {
        generateRoads = FindObjectOfType<GenerateRoads>();
        player = FindObjectOfType<Player>();
        itemManager = FindObjectOfType<ItemManager>();
        bonusSpawnRates = FindObjectOfType<BonusSpawnRates>();
        spawnObjRandPos = FindObjectOfType<SpawnObjRandPos>();

        upgradeDifficultyHolder = distanceUpgradeDifficulty;

        //if save file doesn't exist : create one with values = 0
        SaveSystem.InitiateDataFile();
    }

    private void Update() {
        if (gameState == GameState.PLAYING) {
            AddScore();
            DistanceUpdate();
        }
    }
    #endregion

    #region Methods
    public void StartPlaying() {
        gameState = GameState.PLAYING;

        itemManager.StartSpawnItems();
        spawnObjRandPos.StartSpawnLateralObjects();

        player.StartRunning();
    }

    public void StartAnimation() {
        gameState = GameState.ANIMATION_START;
    }

    public void Lose() {
        UpdatePlayerStats();
        gameState = GameState.FINISHED;
        //SceneManager.LoadScene(0);
    }

    private void AddScore() {
        if (player.isOvni) {
            score += Time.timeScale * generateRoads.roadSpeed * COEF_UNIT_METER * ovniMultiplier * Time.deltaTime;
        }
        else {
            score += Time.timeScale * generateRoads.roadSpeed * COEF_UNIT_METER * Time.deltaTime;
        }
    }

    private void DistanceUpdate() {
        traveledDistance += Time.timeScale * generateRoads.roadSpeed * COEF_UNIT_METER * Time.deltaTime;

        //if (Mathf.Approximately(traveledDistance % distanceUpgradeDifficulty, 0f)) {
        //    Time.timeScale += timescaleDifficulty;
        //    Debug.Log("difficulty up");
        //}

        if (traveledDistance >= distanceUpgradeDifficulty) {    //upgrade difficulty according to travaled distance
            Time.timeScale += timescaleDifficulty;
            distanceUpgradeDifficulty += upgradeDifficultyHolder;
            Debug.Log("difficulty up");
        }

        if (traveledDistance >= distanceStartSpawningTmax) {                    //last bonus spawn
            bonusSpawnRates.ChangeTmaxSpawnRate(TmaxSpawnRate_3);
            bonusSpawnRates.ChangeTwingoSpawnRate(TwingoSpawnRate_3);
            bonusSpawnRates.ChangePochonSpawnRate(pochonSpawnRate_3);
            bonusSpawnRates.ChangeClaquetteSpawnRate(claquetteSpawnRate_3);
        }
        else if (traveledDistance >= distanceStartSpawningTwingo) {             //second bonus spawn
            bonusSpawnRates.ChangeTwingoSpawnRate(TwingoSpawnRate_2);
            bonusSpawnRates.ChangePochonSpawnRate(pochonSpawnRate_2);
            bonusSpawnRates.ChangeClaquetteSpawnRate(claquetteSpawnRate_2);
        }
        else if (traveledDistance >= distanceStartSpawningPochonClaquette) {     //first bonus spawn
            itemManager.EnableBonus();
            bonusSpawnRates.ChangePochonSpawnRate(pochonSpawnRate_1);
            bonusSpawnRates.ChangeClaquetteSpawnRate(claquetteSpawnRate_1);
        }
    }

    //public void ResetTraveledDistance() {
    //    traveledDistance = 0f;
    //}

    public void AddDodgeScore(bool streak) {
        if (streak) {
            dodgeMultiplier *= 2;
        }
        else {
            dodgeMultiplier = 1;
        }

        score += 5f * dodgeMultiplier;
        Debug.Log("score + " + 5 * dodgeMultiplier);
    }

    public void AddDiscScore(bool isGold) {
        if (isGold) {
            score += 1f;
            nbGoldDiscs++;
        }
        else {
            score += 50f;
            nbPlatDiscs++;
        }
    }

    private void UpdatePlayerStats() {
        PlayerData currentData = SaveSystem.LoadData();
        Debug.Log(string.Format("current gold : {0}, current plat : {1}, current best score : {2}",
            currentData.nbGoldDiscs, currentData.nbPlatDiscs, currentData.bestScore));
        int totalGoldDiscs = currentData.nbGoldDiscs + nbGoldDiscs;
        int platFromGoldDisc = totalGoldDiscs / 1000;

        totalGoldDiscs = totalGoldDiscs % 1000;
        int totalPlatDiscs = currentData.nbPlatDiscs + nbPlatDiscs + platFromGoldDisc;
        int bestScore = Mathf.Max(currentData.bestScore, Mathf.FloorToInt(score));

        PlayerData playerData = new PlayerData(totalGoldDiscs, totalPlatDiscs, bestScore);
        Debug.Log(string.Format("new gold : {0}, new plat : {1}, new best score : {2}", playerData.nbGoldDiscs, playerData.nbPlatDiscs, playerData.bestScore));

        SaveSystem.SavePlayer(playerData);
    }

    [Button("ResetDataFile", "Reset data file", BindingFlags.NonPublic | BindingFlags.Instance)] public int test1;
    private void ResetDataFile() {
        Debug.Log("Data file reset");

        SaveSystem.SavePlayer(new PlayerData(0, 0, 0));
    }

    #endregion
}