using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    private InputManager inputManager;
    private Camera mainCamera;
    private SpawnAlien spawnAlien;

    public Animator animator;
    public GameObject triggerVfxAlienStop;
    public GameObject triggerVfxAlienTmax;

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
            //inputManager.isAlienClickable = true;
            spawnAlien.TouchAlien();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "AlienTriggerCollider")
        {
            spawnAlien.TouchAlien();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "AlienTriggerCollider") {
            //inputManager.isAlienClickable = false;
        }
    }

    private void OnDestroy() {
        GameEvents.current.onAlienFail -= OnAlienFail;
    }

    private void OnAlienFail() {
        spawnAlien.DestroyAliens();
    }
}
