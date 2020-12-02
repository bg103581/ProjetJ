using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventManager : MonoBehaviour
{
    private GameManager gameManager;

    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void StartPlaying() {
        gameManager.StartPlaying();
    }
}
