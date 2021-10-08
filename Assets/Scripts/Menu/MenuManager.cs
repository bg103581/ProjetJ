using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering;
using EasyMobile;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject inGameUI;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject loseUI;
    [SerializeField] private GameObject quitConfirmUI;
    [SerializeField] private GameObject wastedUI;
    [SerializeField] private GameObject howToPlayUI;
    [SerializeField] private TMP_Text resumeCountdownText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text diamondText;
    [SerializeField] private TMP_Text liveScoreText;
    [SerializeField] private TMP_Text liveGoldText;
    [SerializeField] private TMP_Text pauseScoreText;
    [SerializeField] private TMP_Text pauseGoldText;
    [SerializeField] private TMP_Text loseScoreText;
    [SerializeField] private TMP_Text loseGoldText;
    [SerializeField] private TMP_Text loseDiamondText;
    [SerializeField] private TMP_Text loseBestScoreText;
    [SerializeField] private GameObject buttonPause;
    [SerializeField] private Button soundMainMenuButton;
    [SerializeField] private Button musicMainMenuButton;
    [SerializeField] private Button soundPauseButton;
    [SerializeField] private Button musicPauseButton;
    [SerializeField] private Button soundLoseButton;
    [SerializeField] private Button musicLoseButton;
    [SerializeField] private Button languageButton;
    [SerializeField] private Sprite soundOnSprite;
    [SerializeField] private Sprite soundOffSprite;
    [SerializeField] private Sprite musicOnSprite;
    [SerializeField] private Sprite musicOffSprite;
    [SerializeField] private Sprite languageEngSprite;
    [SerializeField] private Sprite languageFraSprite;
    [SerializeField] private Volume volume;

    [SerializeField] private TMP_Text interstitialText;
    [SerializeField] private TMP_Text rewardText;

    [SerializeField] private Button continueButton;

    [SerializeField] private LanguageManager languageManager;

    private GameManager gameManager;

    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();

        GameEvents.current.onReplayButtonTrigger += OnReplay;
        GameEvents.current.onMainMenuButtonTrigger += OnMainMenu;
        GameEvents.current.onPauseButtonTrigger += OnPause;
        GameEvents.current.onLoseGame += OnLoseGame;
    }

    private void Start() {
        DisplayMainMenu();
    }

    private void Update() {
        int liveScore = (int)gameManager.score;
        liveScoreText.SetText(liveScore.ToString());
        liveGoldText.SetText(gameManager.nbGoldDiscs.ToString());

        if (Advertising.IsInterstitialAdReady())
        {
            interstitialText.text = "interstitial ad ready";
            interstitialText.color = Color.green;
        }
        else
        {
            interstitialText.text = "interstitial ad not ready";
            interstitialText.color = Color.red;
        }

        if (Advertising.IsRewardedAdReady())
        {
            rewardText.text = "reward ad ready";
            rewardText.color = Color.green;
        }
        else
        {
            rewardText.text = "reward ad not ready";
            rewardText.color = Color.red;
        }
    }

    private void OnDestroy() {
        GameEvents.current.onReplayButtonTrigger -= OnReplay;
        GameEvents.current.onMainMenuButtonTrigger -= OnMainMenu;
        GameEvents.current.onPauseButtonTrigger -= OnPause;
        GameEvents.current.onLoseGame -= OnLoseGame;
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
        StartCoroutine(CountDown());
    }

    public void Stay() {
        QuitConfirmToPause();
    }

    public void PauseQuit() {
        PauseToQuitConfirm();
    }

    public void ConfirmQuit() {
        GameEvents.current.GoToMainMenu();
        QuitConfirmToMainMenu();
    }

    public void SoundButton() {
        PlayerData currentData = SaveSystem.LoadData();
        PlayerData playerData;

        if (currentData.isSoundActive) {    //turn off sound
            TurnOnOffSoundUI(false);
            SoundManager.current.MuteSounds();

            playerData = new PlayerData(currentData.nbGoldDiscs, currentData.nbDiamDiscs, currentData.bestScore, 
                false, currentData.isMusicActive, currentData.language, currentData.nbAlien);
        }
        else {  //turn on sound
            TurnOnOffSoundUI(true);
            SoundManager.current.DemuteSounds();

            playerData = new PlayerData(currentData.nbGoldDiscs, currentData.nbDiamDiscs, currentData.bestScore, 
                true, currentData.isMusicActive, currentData.language, currentData.nbAlien);
        }

        SaveSystem.SavePlayer(playerData);
    }

    public void MusicButton() {
        PlayerData currentData = SaveSystem.LoadData();
        PlayerData playerData;

        if (currentData.isMusicActive) {    //turn off music
            TurnOnOffMusicUI(false);
            SoundManager.current.MuteMusic();

            playerData = new PlayerData(currentData.nbGoldDiscs, currentData.nbDiamDiscs, currentData.bestScore, 
                currentData.isSoundActive, false, currentData.language, currentData.nbAlien);
        }
        else {  //turn on music
            TurnOnOffMusicUI(true);
            SoundManager.current.DemuteMusic();

            playerData = new PlayerData(currentData.nbGoldDiscs, currentData.nbDiamDiscs, currentData.bestScore, 
                currentData.isSoundActive, true, currentData.language, currentData.nbAlien);
        }

        SaveSystem.SavePlayer(playerData);
    }

    public void LanguageButton()
    {
        PlayerData currentData = SaveSystem.LoadData();
        PlayerData playerData;

        if (currentData.language == Language.FRA)
        {
            languageManager.SwitchLanguage(Language.ENG);
            SwitchLanguageButtonDisplay(true);
            playerData = new PlayerData(currentData.nbGoldDiscs, currentData.nbDiamDiscs, currentData.bestScore, currentData.isSoundActive, false, 
                Language.ENG, currentData.nbAlien);
        }
        else
        {
            languageManager.SwitchLanguage(Language.FRA);
            SwitchLanguageButtonDisplay(false);
            playerData = new PlayerData(currentData.nbGoldDiscs, currentData.nbDiamDiscs, currentData.bestScore, currentData.isSoundActive, false,
                Language.FRA, currentData.nbAlien);
        }

        SaveSystem.SavePlayer(playerData);
    }

    public void LeaderboardButton()
    {
        AdMobManager.current.ShowLeaderboard();
    }

    public void ShowBannerButton()
    {
        AdMobManager.current.ShowBannerAds();
    }

    public void ShowInterstitialButton()
    {
        AdMobManager.current.ShowInterstitialAds();
    }

    public void ShowRewardButton()
    {
        AdMobManager.current.ShowRewardedAds();
    }

    public void Continue()
    {
        LoseToInGame();
        GameEvents.current.PreContinueGame();
        StartCoroutine(CountDownContinue());
    }

	public void HowToPlayButton()
	{
		MainMenuToHowToPlayUI();
	}

	public void BackToMenuButton()
	{
		HowToPlayToMainMenuUI();
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

    private void OnLoseGame() {
        StartCoroutine(Wasted());
    }

    public void SetActiveButtonPause(bool isActive) {
        buttonPause.SetActive(isActive);
    }

    private IEnumerator CountDown() {
        resumeCountdownText.SetText("3");
        yield return new WaitForSecondsRealtime(0.5f);
        resumeCountdownText.SetText("2");
        yield return new WaitForSecondsRealtime(0.5f);
        resumeCountdownText.SetText("1");
        yield return new WaitForSecondsRealtime(0.5f);
        resumeCountdownText.SetText("");
        //event
        GameEvents.current.ResumeGame();
    }

    private IEnumerator CountDownContinue()
    {
        resumeCountdownText.SetText("3");
        yield return new WaitForSecondsRealtime(0.5f);
        resumeCountdownText.SetText("2");
        yield return new WaitForSecondsRealtime(0.5f);
        resumeCountdownText.SetText("1");
        yield return new WaitForSecondsRealtime(0.5f);
        resumeCountdownText.SetText("");
        //event
        GameEvents.current.ContinueGame();
    }

    private IEnumerator Wasted() {
        InGameToWasted();

        //saturation black and white
        UnityEngine.Rendering.Universal.ColorAdjustments colorAdjustments;
        volume.profile.TryGet(out colorAdjustments);
        colorAdjustments.saturation.value = -100;

        yield return new WaitForSecondsRealtime(2f);

        //saturation back to normal
        colorAdjustments.saturation.value = 0;

        DisplayLoseScore();
        WastedToLoseUI();
    }

    private void DisplayMainMenu() {
        PlayerData currentData = SaveSystem.LoadData();

        scoreText.text = currentData.bestScore.ToString();
        diamondText.text = currentData.nbDiamDiscs.ToString();

        if (currentData.isSoundActive) TurnOnOffSoundUI(true);
        else TurnOnOffSoundUI(false);

        if (currentData.isMusicActive) TurnOnOffMusicUI(true);
        else TurnOnOffMusicUI(false);

        if (currentData.language == Language.FRA)
        {
            languageManager.SwitchLanguage(Language.FRA);
            SwitchLanguageButtonDisplay(false);
        }
        else
        {
            languageManager.SwitchLanguage(Language.ENG);
            SwitchLanguageButtonDisplay(true);
        }
    }

    private void DisplayPauseScore() {
        int liveScore = (int)gameManager.score;
        pauseScoreText.SetText(liveScore.ToString());
        pauseGoldText.SetText(gameManager.nbGoldDiscs.ToString());
    }

    private void DisplayLoseScore() {
        PlayerData currentData = SaveSystem.LoadData();
        int liveScore = (int)gameManager.score;

        loseScoreText.SetText(liveScore.ToString());
        loseGoldText.SetText(gameManager.nbGoldDiscs.ToString());
        loseDiamondText.SetText(currentData.nbDiamDiscs.ToString());
        loseBestScoreText.SetText(currentData.bestScore.ToString());
    }

    private void TurnOnOffSoundUI(bool isOn) {
        if (isOn) {
            soundMainMenuButton.image.sprite = soundOnSprite;
            soundPauseButton.image.sprite = soundOnSprite;
            soundLoseButton.image.sprite = soundOnSprite;
        }
        else {
            soundMainMenuButton.image.sprite = soundOffSprite;
            soundPauseButton.image.sprite = soundOffSprite;
            soundLoseButton.image.sprite = soundOffSprite;
        }
    }

    private void TurnOnOffMusicUI(bool isOn) {
        if (isOn) {
            musicMainMenuButton.image.sprite = musicOnSprite;
            musicPauseButton.image.sprite = musicOnSprite;
            musicLoseButton.image.sprite = musicOnSprite;
        }
        else {
            musicMainMenuButton.image.sprite = musicOffSprite;
            musicPauseButton.image.sprite = musicOffSprite;
            musicLoseButton.image.sprite = musicOffSprite;
        }
    }

    private void SwitchLanguageButtonDisplay(bool isFr)
    {
        if (isFr)
        {
            languageButton.image.sprite = languageEngSprite;
        }
        else
        {
            languageButton.image.sprite = languageFraSprite;
        }
    }

    public void StopContinueButton()
    {
        continueButton.interactable = false;
    }

    public void ResetContinueButton()
    {
        continueButton.interactable = true;
    }

    public void MainMenuToInGame() {
        ChangeUI(mainMenuUI, inGameUI);
    }

    //private void InGameToLose() {
    //    ChangeUI(inGameUI, loseUI);
    //    DisplayLoseScore();
    //}

    private void LoseToInGame() {
        ChangeUI(loseUI, inGameUI);
    }

    private void LoseToMainMenu() {
        ChangeUI(loseUI, mainMenuUI);
        DisplayMainMenu();
    }

    private void InGameToPause() {
        ChangeUI(inGameUI, pauseUI);
        DisplayPauseScore();
    }

    private void PauseToInGame() {
        ChangeUI(pauseUI, inGameUI);
    }

    private void PauseToQuitConfirm() {
        ChangeUI(pauseUI, quitConfirmUI);
    }

    private void QuitConfirmToPause() {
        ChangeUI(quitConfirmUI, pauseUI);
    }

    private void QuitConfirmToMainMenu() {
        ChangeUI(quitConfirmUI, mainMenuUI);
    }

    private void InGameToWasted() {
        ChangeUI(inGameUI, wastedUI);
    }

    private void WastedToLoseUI() {
        ChangeUI(wastedUI, loseUI);
    }

	private void MainMenuToHowToPlayUI()
	{
		ChangeUI(mainMenuUI, howToPlayUI);
	}

	private void HowToPlayToMainMenuUI()
	{
		ChangeUI(howToPlayUI, mainMenuUI);
	}

	private void ChangeUI(GameObject uiToDisable, GameObject uiToEnable) {
        uiToDisable.SetActive(false);
        uiToEnable.SetActive(true);
    }
}
