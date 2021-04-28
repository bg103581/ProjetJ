using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveItems))]
public class Disc : MonoBehaviour
{
    public bool isLastOvniDisc = false;
    public bool isOvniDisc = false;

    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void OnEnable() {
        GameEvents.current.onAlienFail += OnAlienFail;
    }

    private void OnDestroy() {
        EndPlayerOvni();

        GameEvents.current.onAlienFail -= OnAlienFail;
    }

    private void OnDisable() {
        Debug.LogWarning("disc disabled; isLastOvniDisc = " + isLastOvniDisc);
        EndPlayerOvni();

        GameEvents.current.onAlienFail -= OnAlienFail;
    }

    private void OnAlienFail() {
        if (isOvniDisc) {
            ObjectPooler.current.DestroyObject(gameObject);
            //Destroy(gameObject);
        }
    }

    private void EndPlayerOvni() {
        if (isLastOvniDisc) {
            Debug.LogWarning("end player ovni");
            player.EndOvni();
        }
    }
}
