using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        switch (tag) {
            case "Pochon":
                gameManager.isPochonInGame = true;
                break;
            case "Claquettes":
                gameManager.isClaquetteInGame = true;
                break;
            case "Twingo":
                gameManager.isTwingoInGame = true;
                break;
            case "Tmax":
                gameManager.isTmaxInGame = true;
                break;
            default:
                break;
        }
    }

    private void OnDestroy() {
        switch (tag) {
            case "Pochon":
                gameManager.isPochonInGame = false;
                break;
            case "Claquettes":
                gameManager.isClaquetteInGame = false;
                break;
            case "Twingo":
                gameManager.isTwingoInGame = false;
                break;
            case "Tmax":
                gameManager.isTmaxInGame = false;
                break;
            default:
                break;
        }
    }
}
