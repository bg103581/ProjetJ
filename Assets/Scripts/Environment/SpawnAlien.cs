using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpawnAlien : MonoBehaviour
{
    private Player player;
    private InputManager inputManager;

    private GameObject alienSpawned;

    private Lane alienLane;

    private bool isSpawnAlien = false;

    private int aliensCurrentIndex = 0;

    [Tooltip("Aliens ordered in spawn apparition")]
    [SerializeField]
    private GameObject[] aliens;    //ordered in spawn apparition
    [SerializeField]
    private Transform leftAlienPos;
    [SerializeField]
    private Transform rightAlienPos;
    [SerializeField]
    private Transform upLeftAlienPos;
    [SerializeField]
    private Transform upRightAlienPos;
    [SerializeField]
    private Transform tmaxFirstPos;
    [SerializeField]
    private float moveToTmaxTimer;

    private void Awake() {
        player = FindObjectOfType<Player>();
        inputManager = FindObjectOfType<InputManager>();
    }

    public void StartAlienSpawn() {
        float spawnTime = Random.Range(0, player.tmaxDuration - 4f);
        Debug.Log("start alien spawn");
        if (!isSpawnAlien)
            Invoke("SpawnNextAlien", spawnTime);
        isSpawnAlien = true;
    }

    public void SpawnNextAlien() {
        Transform spawnPos;
        int rand = Random.Range(0, 2);
        if (rand == 0) {
            if (aliensCurrentIndex == 0)
                spawnPos = leftAlienPos;
            else
                spawnPos = upLeftAlienPos;
            alienLane = Lane.LEFT;
        }
        else {
            if (aliensCurrentIndex == 0)
                spawnPos = rightAlienPos;
            else
                spawnPos = upRightAlienPos;
            alienLane = Lane.RIGHT;
        }

        if (aliensCurrentIndex < aliens.Length) {
            alienSpawned = Instantiate(aliens[aliensCurrentIndex], spawnPos.position, spawnPos.rotation, transform);
            aliensCurrentIndex++;
        }
    }

    public void TouchAlien() {
        if (player.lane == alienLane) {
            inputManager.isAlienClickable = false;

            alienSpawned.GetComponent<MoveItems>().enabled = false;
            //do move alien on tmax
            alienSpawned.transform.SetParent(tmaxFirstPos);
            alienSpawned.transform.DOLocalMove(Vector3.zero, moveToTmaxTimer);
            alienSpawned.transform.DOLocalRotate(Vector3.zero, moveToTmaxTimer);
            if (aliensCurrentIndex == 3) {
                player.StartOvni();
            }
            else {
                //jul fly
                player.Fly();
            }
        }
    }
}
