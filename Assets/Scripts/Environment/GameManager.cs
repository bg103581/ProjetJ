using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Reflection;
using DG.Tweening;

public enum GameState { WAITING, ANIMATION_START, PLAYING, FINISHED, PAUSE }

public class GameManager : MonoBehaviour
{
    #region Variables
    private const float COEF_UNIT_METER = 0.583f;
    
    private GenerateRoads generateRoads;
    private Player player;
    private Cop cop;
    private ItemManager itemManager;
    private BonusSpawnRates bonusSpawnRates;
    private SpawnObjRandPos spawnObjRandPos;
    private InputManager inputManager;
    private MenuManager menuManager;

    private int dodgeMultiplier = 1;
    private int ovniMultiplier = 2;
    
    //private float upgradeDifficultyHolder;
    private float currentTimeScale = 1;
    [SerializeField] private float runTimer = 0;
    [SerializeField] private float rawTimeScaleHolder = 1;

    private bool startRunTimer = false;
    private bool isRunTimerCounting = false;
    private bool maxSpeedReached = false;
    private bool maxItemRatesStepReached = false;

    [HideInInspector]
    public bool isPochonInGame;
    [HideInInspector]
    public bool isClaquetteInGame;
    [HideInInspector]
    public bool isTwingoInGame;
    [HideInInspector]
    public bool isTmaxInGame;

    public float score = 0;
    public int nbGoldDiscs = 0;

    [HideInInspector]
    public GameState gameState = GameState.WAITING;
    
    [SerializeField]
    private float traveledDistance = 0;
    [SerializeField]
    private float freqToUpgradeDifficulty;
    [SerializeField]
    private float timeToUpgradeDifficulty;
    [SerializeField]
    private float timescaleDifficulty;
    [SerializeField]
    private float maxSpeedTimeScale;
    [SerializeField]
    private float breakItemBonus;
    [Header("Timers to change items rates")]
    [SerializeField]
    private float mediumItemRatesTime;
    [SerializeField]
    private float lastItemRatesTime;

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

    #endregion

    #region MonoBehaviour
    private void Awake() {
        GameEvents.current.onReplayButtonTrigger += OnReplay;
        GameEvents.current.onMainMenuButtonTrigger += ResetValues;
        GameEvents.current.onPauseButtonTrigger += OnPause;
        GameEvents.current.onResumeTrigger += OnResume;
    }

    private void Start() {
        generateRoads = FindObjectOfType<GenerateRoads>();
        player = FindObjectOfType<Player>();
        cop = FindObjectOfType<Cop>();
        itemManager = FindObjectOfType<ItemManager>();
        bonusSpawnRates = FindObjectOfType<BonusSpawnRates>();
        spawnObjRandPos = FindObjectOfType<SpawnObjRandPos>();
        inputManager = FindObjectOfType<InputManager>();
        menuManager = FindObjectOfType<MenuManager>();

        timeToUpgradeDifficulty = freqToUpgradeDifficulty;

        //if save file doesn't exist : create one with values = 0
        SaveSystem.InitiateDataFile();
    }

    private void Update() {
        if (gameState == GameState.PLAYING) {
            AddScore();
            DistanceUpdate();
            DifficultyUpdate();
            BonusUpdate();
        }
    }

    private void OnDestroy() {
        GameEvents.current.onReplayButtonTrigger -= OnReplay;
        GameEvents.current.onMainMenuButtonTrigger -= ResetValues;
        GameEvents.current.onPauseButtonTrigger -= OnPause;
        GameEvents.current.onResumeTrigger -= OnResume;
    }
    #endregion

    #region Methods
    public void StartAnimation() {
        gameState = GameState.ANIMATION_START;
        player.StartAnimation();
        cop.StartAnimation();

        //changer main menu ui en in game ui
        menuManager.MainMenuToInGame();
    }

    public void StartPlaying() {    //start the game (spawns, road and items movements)
        gameState = GameState.PLAYING;

        itemManager.StartSpawnItems();
        spawnObjRandPos.StartSpawnLateralObjects();
    }

    public void StartPlayingInputs() {  //the player start playing
        inputManager.isRegisteringInputs = true;
        startRunTimer = true;

        player.startCopFollowTimer = true;
        cop.CatchUpToPlayer();

        menuManager.SetActiveButtonPause(true);
    }

    public void Lose() {
        GameEvents.current.LoseGame();
        Time.timeScale = 1;
        UpdatePlayerStats();
        gameState = GameState.FINISHED;
        isRunTimerCounting = false;

        //menuManager.InGameToLose();
    }

    private void OnPause() {
        gameState = GameState.PAUSE;
        currentTimeScale = Time.timeScale;
        Time.timeScale = 0;

        menuManager.SetActiveButtonPause(false);
    }

    private void OnResume() {
        gameState = GameState.PLAYING;
        Time.timeScale = currentTimeScale;

        menuManager.SetActiveButtonPause(true);
    }

    private void OnReplay() {   //reset game values then launch start animation
        ResetValues();
        StartCoroutine("ReplayCorout");
    }

    private void ResetValues() {
        DOTween.Clear();

        gameState = GameState.WAITING;
        score = 0;
        nbGoldDiscs = 0;
        traveledDistance = 0;
        runTimer = 0;
        startRunTimer = false;
        isRunTimerCounting = false;
        Time.timeScale = 1;
        rawTimeScaleHolder = 1;
        timeToUpgradeDifficulty = freqToUpgradeDifficulty;
        menuManager.SetActiveButtonPause(false);
    }

    private IEnumerator ReplayCorout() {
        yield return new WaitForEndOfFrame();
        StartAnimation();
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

        //if (traveledDistance >= distanceUpgradeDifficulty) {    //upgrade difficulty according to travaled distance
        //    Time.timeScale += timescaleDifficulty;
        //    distanceUpgradeDifficulty += upgradeDifficultyHolder;
        //    Debug.Log("difficulty up");
        //}

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

    private void DifficultyUpdate() {
        if (startRunTimer) {
            isRunTimerCounting = true;
            runTimer = 0;

            startRunTimer = false;
        }

        if (isRunTimerCounting) {
            if (gameState != GameState.PAUSE && (!maxSpeedReached || !maxItemRatesStepReached)) {
                runTimer += Time.unscaledDeltaTime;

                if (runTimer >= timeToUpgradeDifficulty) {
                    Debug.Log("upgrade difficulty");
                    rawTimeScaleHolder += timescaleDifficulty;
                    Time.timeScale += timescaleDifficulty;
                    timeToUpgradeDifficulty += freqToUpgradeDifficulty;
                }

                if (rawTimeScaleHolder >= maxSpeedTimeScale) {
                    Debug.Log("max speed reached");
                    maxSpeedReached = true;
                }

                if (runTimer >= lastItemRatesTime) {
                    itemManager.ChangeItemRates(3);
                    maxItemRatesStepReached = true;
                }
                else if (runTimer >= mediumItemRatesTime) {
                    itemManager.ChangeItemRates(2);
                }
            }
        }
    }

    private void BonusUpdate() {    //can't spawn same bonuses
        if (player.isPochon || isPochonInGame) {
            bonusSpawnRates.ChangePochonSpawnRate(0);
        }
        if (player.isClaquettes || isClaquetteInGame || player.isTwingo || isTwingoInGame || player.isTmax || isTmaxInGame) {
            bonusSpawnRates.ChangeClaquetteSpawnRate(0);
        }
        if (player.isTwingo || isTwingoInGame || player.isTmax || isTmaxInGame) {   
            bonusSpawnRates.ChangeTwingoSpawnRate(0);
            bonusSpawnRates.ChangeTmaxSpawnRate(0);
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
            nbGoldDiscs = nbGoldDiscs + 50;
        }
    }

    public void AddBreakItemScore(bool isCar = false) {
        if (isCar) {
            score += breakItemBonus * 2;
        }
        else {
            score += breakItemBonus;
        }
    }

    private void UpdatePlayerStats() {
        PlayerData currentData = SaveSystem.LoadData();
        Debug.Log(string.Format("current gold : {0}, current diam : {1}, current best score : {2}",
            currentData.nbGoldDiscs, currentData.nbDiamDiscs, currentData.bestScore));
        int totalGoldDiscs = currentData.nbGoldDiscs + nbGoldDiscs;
        int diamFromGoldDisc = totalGoldDiscs / 1000;

        totalGoldDiscs = totalGoldDiscs % 1000;
        int totalDiamDiscs = currentData.nbDiamDiscs + diamFromGoldDisc;
        int bestScore = Mathf.Max(currentData.bestScore, Mathf.FloorToInt(score));

        PlayerData playerData = new PlayerData(totalGoldDiscs, totalDiamDiscs, bestScore, currentData.isSoundActive, currentData.isMusicActive);
        Debug.Log(string.Format("new gold : {0}, new diam : {1}, new best score : {2}", playerData.nbGoldDiscs, playerData.nbDiamDiscs, playerData.bestScore));

        SaveSystem.SavePlayer(playerData);
    }

    [Button("ResetDataFile", "Reset data file", BindingFlags.NonPublic | BindingFlags.Instance)] public int test1;
    private void ResetDataFile() {
        Debug.Log("Data file reset");

        SaveSystem.SavePlayer(new PlayerData(0, 0, 0, true, true));
    }

    #endregion
}