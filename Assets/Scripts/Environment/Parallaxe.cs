using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Parallaxe : MonoBehaviour
{
    [SerializeField] private Transform firstLayer;
    [SerializeField] private Transform secondLayer;
    [SerializeField] private Transform thirdLayer;

    [SerializeField] private float offSet;

    private Vector3 initFirstLayerPos;
    private Vector3 initSecondLayerPos;
    private Vector3 initThirdLayerPos;

    private float xValue;

    private void Awake() {
        GameEvents.current.onReplayButtonTrigger += OnReset;
        GameEvents.current.onMainMenuButtonTrigger += OnReset;

        initFirstLayerPos = firstLayer.position;
        initSecondLayerPos = secondLayer.position;
        initThirdLayerPos = thirdLayer.position;

        xValue = firstLayer.position.x;
    }

    private void OnDestroy() {
        GameEvents.current.onReplayButtonTrigger -= OnReset;
        GameEvents.current.onMainMenuButtonTrigger -= OnReset;
    }

    public void MoveLeft(float duration) {

        xValue -= offSet;

        firstLayer.DOLocalMoveX(xValue, duration).SetEase(Ease.OutSine);
        secondLayer.DOLocalMoveX(xValue * 2, duration).SetEase(Ease.OutSine);
        thirdLayer.DOLocalMoveX(xValue * 3, duration).SetEase(Ease.OutSine);
    }

    public void MoveRight(float duration) {
        xValue += offSet;

        firstLayer.DOLocalMoveX(xValue, duration).SetEase(Ease.OutSine);
        secondLayer.DOLocalMoveX(xValue * 2, duration).SetEase(Ease.OutSine);
        thirdLayer.DOLocalMoveX(xValue * 3, duration).SetEase(Ease.OutSine);
    }

    private void OnReset() {
        DOTween.Clear();
        firstLayer.position = initFirstLayerPos;
        secondLayer.position = initSecondLayerPos;
        thirdLayer.position = initThirdLayerPos;
        xValue = firstLayer.position.x;
    }
}
