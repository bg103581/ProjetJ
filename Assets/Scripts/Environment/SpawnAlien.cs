using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpawnAlien : MonoBehaviour
{
    private Player player;
    private InputManager inputManager;

    private GameObject alienSpawned;
    private List<GameObject> aliensInGame = new List<GameObject>();

    private Lane alienLane;

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
    private Transform tmaxSecondPos;
    [SerializeField]
    private float moveToTmaxTimer;

    private void Awake() {
        player = FindObjectOfType<Player>();
        inputManager = FindObjectOfType<InputManager>();
    }

    public void StartAlienSpawn() {
        float spawnTime = Random.Range(0, player.tmaxDuration - 4f);
        aliensCurrentIndex = 0;
        Debug.Log("start alien spawn");
        Invoke("SpawnNextAlien", spawnTime);
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
            aliensInGame.Add(alienSpawned);
            aliensCurrentIndex++;
        }
    }

    public void TouchAlien() {
        if (player.lane == alienLane) {
            inputManager.isAlienClickable = false;

            alienSpawned.GetComponent<MoveItems>().enabled = false;
            alienSpawned.GetComponent<CapsuleCollider>().enabled = false;

            //if (aliensCurrentIndex == 3) {
            //    player.StartOvni();
            //}
            //else {
            //    //jul fly
            //    player.Fly();
            //}

            switch (aliensCurrentIndex) {
                case 1:
                    MoveAlienToTmax(tmaxFirstPos);
                    player.Fly();
                    break;
                case 2:
                    MoveAlienToTmax(tmaxSecondPos);
                    player.Fly();
                    break;
                case 3:
                    DestroyAliens();
                    player.StartOvni();
                    break;
                default:
                    break;
            }
        }
    }

    private void MoveAlienToTmax(Transform parent) {
        alienSpawned.transform.SetParent(parent);
        alienSpawned.transform.DOLocalMove(Vector3.zero, moveToTmaxTimer);
        alienSpawned.transform.DOLocalRotate(Vector3.zero, moveToTmaxTimer);
    }

    public void DestroyAliens() {
        if (aliensInGame.Count > 0) {
            foreach (GameObject alien in aliensInGame) {
                Destroy(alien);
            }
            aliensInGame.Clear();
        }
    }
}
