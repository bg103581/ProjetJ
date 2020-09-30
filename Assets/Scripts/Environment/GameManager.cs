using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : GenericSingleton<GameManager>
{
    #region Variables
    private const float COEF_UNIT_METER = 0.583f;
    
    private GenerateRoads generateRoads;

    private int dodgeMultiplier = 1;

    [SerializeField]
    private float score = 0;
    
    #endregion

    #region MonoBehaviour
    private void Start() {
        generateRoads = FindObjectOfType<GenerateRoads>();
    }

    private void Update() {
        score += Time.timeScale * generateRoads.roadSpeed * COEF_UNIT_METER * Time.deltaTime;
    }
    #endregion

    #region Methods
    public void Lose() {
        SceneManager.LoadScene(0);
    }

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
        if (isGold)
            score += 1f;
        else
            score += 50f;
    }    
    #endregion
}