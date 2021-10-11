using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LanguageManager : MonoBehaviour
{
    [SerializeField] private TMP_Text playButtonText;
    [SerializeField] private TMP_Text leaderboardButtonText;
    [SerializeField] private TMP_Text actualScore;
    [SerializeField] private TMP_Text goldRecordsPauseMenu;
    [SerializeField] private TMP_Text resumeButtonText;
    [SerializeField] private TMP_Text quitPauseButtonText;
    [SerializeField] private TMP_Text goldRecordsLoseMenu;
    [SerializeField] private TMP_Text diamondRecords;
    [SerializeField] private TMP_Text personalBest;
    [SerializeField] private TMP_Text continueText;
    [SerializeField] private TMP_Text continueButtonText;
    [SerializeField] private TMP_Text playAgainButtonText;
    [SerializeField] private TMP_Text quitLoseButtonText;
    [SerializeField] private TMP_Text firstQuitConfirmText;
    [SerializeField] private TMP_Text secondQuitConfirmText;
    [SerializeField] private TMP_Text stayButtonText;
    [SerializeField] private TMP_Text quitConfirmButtonText;
    [SerializeField] private TMP_Text howToPlayButtonText;
    [SerializeField] private TMP_Text swipeUpJumpText;
    [SerializeField] private TMP_Text swipeSidesText;
    [SerializeField] private TMP_Text swipeUpWheelieText;
    [SerializeField] private TMP_Text collectRecordsText;
    [SerializeField] private TMP_Text backToMenuButtonText;

    public void SwitchLanguage(Language language)
    {
        if (language == Language.FRA)
        {
            playButtonText.text = "JOUER";
            leaderboardButtonText.text = "CLASSEMENT";
            actualScore.text = "Score Actuel";
            goldRecordsPauseMenu.text = "Disques d'or";
            resumeButtonText.text = "CONTINUER";
            quitPauseButtonText.text = "QUITTER";
            goldRecordsLoseMenu.text = "Disques d'or";
            diamondRecords.text = "Disques de diamant";
            personalBest.text = "Meilleur Score";
            continueText.text = "Reprenez votre course en regardant une vidéo";
            continueButtonText.text = "CONTINUER";
            playAgainButtonText.text = "REJOUER";
            quitLoseButtonText.text = "QUITTER";
            firstQuitConfirmText.text = "La progression ne sera pas enregistrée.";
            secondQuitConfirmText.text = "Etes-vous sûr de vouloir quitter ?";
            stayButtonText.text = "REPRENDRE";
            quitConfirmButtonText.text = "QUITTER";
			howToPlayButtonText.text = "COMMENT JOUER";
			swipeUpJumpText.text = "Glisse vers le haut \npour sauter";
			swipeSidesText.text = "Glisse à gauche \nou à droite pour \nchanger de voie";
			swipeUpWheelieText.text = "Glisse vers le haut \npour mettre en Y";
			collectRecordsText.text = "Collecte le plus de \ndisques d'or possible";
			backToMenuButtonText.text = "RETOUR AU MENU";
		}
        else
        {
            playButtonText.text = "PLAY";
            leaderboardButtonText.text = "LEADERBOARD";
            actualScore.text = "Actual Score";
            goldRecordsPauseMenu.text = "Gold records";
            resumeButtonText.text = "RESUME";
            quitPauseButtonText.text = "QUIT";
            goldRecordsLoseMenu.text = "Gold records";
            diamondRecords.text = "Diamond records";
            personalBest.text = "Personal best";
            continueText.text = "Resume your actual run by watching a video";
            continueButtonText.text = "CONTINUE";
            playAgainButtonText.text = "PLAY AGAIN";
            quitLoseButtonText.text = "QUIT";
            firstQuitConfirmText.text = "Progress won't be saved.";
            secondQuitConfirmText.text = "Are you sure you want to leave ?";
            stayButtonText.text = "STAY";
            quitConfirmButtonText.text = "QUIT";
			howToPlayButtonText.text = "HOW TO PLAY";
			swipeUpJumpText.text = "Swipe UP \nto Jump";
			swipeSidesText.text = "Swipe LEFT or RIGHT \nto switch lane";
			swipeUpWheelieText.text = "Swipe UP \nto do a wheelie";
			collectRecordsText.text = "Collect as many gold \nrecords as possible";
			backToMenuButtonText.text = "BACK TO MENU";
		}
    }
}
