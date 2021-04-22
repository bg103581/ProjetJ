using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscPatterns : MonoBehaviour
{
    private void Awake() {
        GameEvents.current.onReplayButtonTrigger += OnReplay;
    }

    private void Start()
    {
        StartCoroutine(AutoDestroy());
    }

    private IEnumerator AutoDestroy()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    private void OnReplay() {
        Destroy(gameObject);
    }

    private void OnDestroy() {
        GameEvents.current.onReplayButtonTrigger -= OnReplay;
    }
}
