using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{

    public int nbGoldDiscs;
    public int nbDiamDiscs;

    public int bestScore;

    public PlayerData (int gold, int diam, int score) {
        nbGoldDiscs = gold;
        nbDiamDiscs = diam;
        bestScore = score;
    }

}
