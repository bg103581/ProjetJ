using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{

    public int nbGoldDiscs;
    public int nbPlatDiscs;

    public int bestScore;

    public PlayerData (int gold, int plat, int score) {
        nbGoldDiscs = gold;
        nbPlatDiscs = plat;
        bestScore = score;
    }

}
