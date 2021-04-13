using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjRandPos : GeneratePrefabs
{
    [SerializeField]
    private Transform rightSpawnPosition;
    [SerializeField]
    private Transform leftSpawnPosition;
    [SerializeField]
    private float spawnFreq;

    private GameManager gameManager;

    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();

        GameEvents.current.onReplayButtonTrigger += OnReplay;
        GameEvents.current.onMainMenuButtonTrigger += OnReplay;
        GameEvents.current.onLoseGame += OnLose;
    }

    private void OnDestroy() {
        GameEvents.current.onReplayButtonTrigger -= OnReplay;
        GameEvents.current.onMainMenuButtonTrigger -= OnReplay;
        GameEvents.current.onLoseGame -= OnLose;
    }

    private void OnReplay() {
        CancelInvoke();
    }

    private void OnLose() {
        Animator anim = GetComponentInChildren<Animator>();
        if (anim != null) anim.enabled = false;
    }

    public void StartSpawnLateralObjects() {
        InvokeRepeating("SpawnObj", spawnFreq, spawnFreq);
    }

    private void SpawnObj() {
        if (gameManager.gameState == GameState.PLAYING) {
            Transform spawnPos;
            bool isRight;
            int rand = Random.Range(0, 2);

            if (rand == 0) {
                spawnPos = rightSpawnPosition;
                isRight = true;
            }
            else {
                spawnPos = leftSpawnPosition;
                isRight = false;
            }

            Quaternion rot;
            GameObject lateralObj;

            if (isRight) {
                rot = Quaternion.Euler(new Vector3(spawnPos.rotation.x, spawnPos.rotation.y + 180, spawnPos.rotation.z));
                lateralObj = Instantiate(GetRandomPrefab(), spawnPos.position, rot, transform);

                lateralObj.GetComponent<Obstacles>().currentLane = Lane.LEFT;
            }
            else {
                rot = spawnPos.rotation;
                lateralObj = Instantiate(GetRandomPrefab(), spawnPos.position, rot, transform);

                lateralObj.GetComponent<Obstacles>().currentLane = Lane.RIGHT;
            }

            if (lateralObj.tag == "Rat") SoundManager.current.PlaySound(SoundType.RAT);
            else if (lateralObj.tag == "Ballon") SoundManager.current.PlaySound(SoundType.BALLON);
            
            lateralObj.GetComponent<LateralMovement>().isStartingRight = isRight;
        }
    }
}
