using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenuUI;
    [SerializeField]
    private GameObject inGameUI;
    [SerializeField]
    private GameObject pauseUI;
    [SerializeField]
    private GameObject loseUI;

    private GameManager gameManager;

    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();

        GameEvents.current.onReplayButtonTrigger += OnReplay;
    }

    private void OnDestroy() {
        GameEvents.current.onReplayButtonTrigger -= OnReplay;
    }


    public void Replay() {  //call replay event
        GameEvents.current.Replay();
    }

    private void OnReplay() {
        LoseToInGame();
    }

    public void MainMenuToInGame() {
        ChangeUI(mainMenuUI, inGameUI);
    }

    public void InGameToLose() {
        ChangeUI(inGameUI, loseUI);
    }

    public void LoseToInGame() {
        ChangeUI(loseUI, inGameUI);
    }

    private void ChangeUI(GameObject uiToDisable, GameObject uiToEnable) {
        uiToDisable.SetActive(false);
        uiToEnable.SetActive(true);
    }
}
