using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Language { ENG, FRA}

[System.Serializable]
public class PlayerData
{
    public int nbGoldDiscs;
    public int nbDiamDiscs;

    public int bestScore;

    public bool isSoundActive;
    public bool isMusicActive;

    public Language language;

    public PlayerData (int gold, int diam, int score, bool sound, bool music, Language lang) {
        nbGoldDiscs = gold;
        nbDiamDiscs = diam;
        bestScore = score;
        isSoundActive = sound;
        isMusicActive = music;
        language = lang;
    }

}
