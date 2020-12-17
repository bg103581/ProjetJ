using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


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
    [SerializeField]
    private TMP_Text resumeCountdownText;

    private GameManager gameManager;

    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();

        GameEvents.current.onReplayButtonTrigger += OnReplay;
        GameEvents.current.onMainMenuButtonTrigger += OnMainMenu;
        GameEvents.current.onPauseButtonTrigger += OnPause;
    }

    private void OnDestroy() {
        GameEvents.current.onReplayButtonTrigger -= OnReplay;
        GameEvents.current.onMainMenuButtonTrigger -= OnMainMenu;
        GameEvents.current.onPauseButtonTrigger -= OnPause;
    }


    public void Replay() {  //call replay event
        GameEvents.current.Replay();
    }

    public void MainMenu() {
        GameEvents.current.GoToMainMenu();
    }

    public void Pause() {
        GameEvents.current.PauseGame();
    }

    public void Resume() {
        PauseToInGame();
        StartCoroutine("CountDown");
    }

    private void OnReplay() {
        LoseToInGame();
    }

    private void OnMainMenu() {
        LoseToMainMenu();
    }

    private void OnPause() {
        InGameToPause();
    }

    private IEnumerator CountDown() {
        resumeCountdownText.SetText("3");
        yield return new WaitForSecondsRealtime(1f);
        resumeCountdownText.SetText("2");
        yield return new WaitForSecondsRealtime(1f);
        resumeCountdownText.SetText("1");
        yield return new WaitForSecondsRealtime(1f);
        resumeCountdownText.SetText("");
        //event
        GameEvents.current.ResumeGame();
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

    public void LoseToMainMenu() {
        ChangeUI(loseUI, mainMenuUI);
    }

    public void InGameToPause() {
        ChangeUI(inGameUI, pauseUI);
    }

    public void PauseToInGame() {
        ChangeUI(pauseUI, inGameUI);
    }

    private void ChangeUI(GameObject uiToDisable, GameObject uiToEnable) {
        uiToDisable.SetActive(false);
        uiToEnable.SetActive(true);
    }
}
