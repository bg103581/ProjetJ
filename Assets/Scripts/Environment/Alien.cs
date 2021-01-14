using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    private InputManager inputManager;
    private Camera mainCamera;

    private void Start() {
        inputManager = FindObjectOfType<InputManager>();
        mainCamera = FindObjectOfType<Camera>();
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
}
