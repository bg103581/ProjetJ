using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventManager : MonoBehaviour
{
    private GameManager gameManager;
    private JulAnim julStartAnimMovement;
    private CameraMovement cameraMovement;

    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();
        julStartAnimMovement = GetComponent<JulAnim>();
        cameraMovement = FindObjectOfType<CameraMovement>();
    }

    public void StartPlaying() {
        gameManager.StartPlaying();
    }

    public void MoveJul() {
        if (transform.name == "Jul") {
            julStartAnimMovement.PlayStartMovement();
            cameraMovement.MoveToPlayPos();
            StartPlaying();
        }
    }

    public void RotateStartPos() {
        julStartAnimMovement.RotatePlayPos();
    }
}
