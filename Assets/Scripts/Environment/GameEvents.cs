using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    private void Awake() {
        current = this;
    }

    public event Action onReplayButtonTrigger;
    public void Replay() {
        if (onReplayButtonTrigger != null) {
            onReplayButtonTrigger();
        }
    }

    public event Action onMainMenuButtonTrigger;
    public void GoToMainMenu() {
        if (onMainMenuButtonTrigger != null) {
            onMainMenuButtonTrigger();
        }
    }

    public event Action onPauseButtonTrigger;
    public void PauseGame() {
        if (onMainMenuButtonTrigger != null) {
            onPauseButtonTrigger();
        }
    }

    public event Action onResumeTrigger;
    public void ResumeGame() {
        if (onResumeTrigger != null) {
            onResumeTrigger();
        }
    }

    public event Action onAlienFail;
    public void AlienFail() {
        if (onResumeTrigger != null) {
            onAlienFail();
        }
    }
}
