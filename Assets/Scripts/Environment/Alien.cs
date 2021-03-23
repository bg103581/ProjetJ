using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    private InputManager inputManager;
    private Camera mainCamera;
    private SpawnAlien spawnAlien;

    public Animator animator;
    public TriggerVfx triggerVfxAlien;

    private void Start() {
        GameEvents.current.onAlienFail += OnAlienFail;

        inputManager = FindObjectOfType<InputManager>();
        mainCamera = FindObjectOfType<Camera>();
        spawnAlien = FindObjectOfType<SpawnAlien>();
    }

    private void Update() {
        inputManager.alienScreenPos = mainCamera.WorldToScreenPoint(transform.position);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "AlienTriggerCollider") {
            Debug.Log("trigger alien");
            inputManager.isAlienClickable = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "AlienTriggerCollider") {
            Debug.Log("isAlienClickable = false");
            inputManager.isAlienClickable = false;
        }
    }

    private void OnDestroy() {
        GameEvents.current.onAlienFail -= OnAlienFail;
    }

    private void OnAlienFail() {
        spawnAlien.DestroyAliens();
    }
}
