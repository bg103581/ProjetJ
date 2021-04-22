using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TmaxLose : MonoBehaviour
{
    [SerializeField] private GameObject tmax;
    [SerializeField] private GameObject tmaxAndJulLose;
    [SerializeField] private Animator tmaxLoseAnim;

    private void Awake() {
        GameEvents.current.onMainMenuButtonTrigger += OnReset;
        GameEvents.current.onReplayButtonTrigger += OnReset;
        GameEvents.current.onPreContinueGame += OnReset;
    }

    private void OnDestroy() {
        GameEvents.current.onMainMenuButtonTrigger -= OnReset;
        GameEvents.current.onReplayButtonTrigger -= OnReset;
        GameEvents.current.onPreContinueGame -= OnReset;
    }

    public void StartLoseAnimation(Lane laneObstacle) {
        tmax.SetActive(false);
        tmaxAndJulLose.SetActive(true);

        switch (laneObstacle) {
            case Lane.LEFT:
                tmaxLoseAnim.Play("tmaxDeathRight");
                break;
            case Lane.CENTER:
                int rand = Random.Range(0, 2);
                if (rand == 0) tmaxLoseAnim.Play("tmaxDeathRight");
                else tmaxLoseAnim.Play("tmaxDeathLeft");
                break;
            case Lane.RIGHT:
                tmaxLoseAnim.Play("tmaxDeathLeft");
                break;
            default:
                tmaxLoseAnim.Play("tmaxDeathRight");
                break;
        }
        
    }

    private void OnReset() {
        tmax.SetActive(true);
        tmaxAndJulLose.SetActive(false);
    }
}
