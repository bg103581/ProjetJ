using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{

    public int nbGoldDiscs;
    public int nbDiamDiscs;

    public int bestScore;

    public bool isSoundActive;
    public bool isMusicActive;

    public PlayerData (int gold, int diam, int score, bool sound, bool music) {
        nbGoldDiscs = gold;
        nbDiamDiscs = diam;
        bestScore = score;
        isSoundActive = sound;
        isMusicActive = music;
    }

}
