using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateDiscs : MonoBehaviour
{
    [SerializeField]
    private ItemManager itemManager;
    [SerializeField]
    private GameObject leftPatternPrefab;
    [SerializeField]
    private GameObject jumpPatternPrefab;
    [SerializeField]
    private GameObject rightPatternPrefab;
    [SerializeField]
    private GameObject linePatternPrefab;
    [SerializeField]
    private GameObject goldenDiscPrefab;
    [SerializeField]
    private GameObject platinumDiscPrefab;
    [SerializeField]
    private int platinumPatternChance;

    //private Transform[] leftPatternTransforms;
    //private Transform[] jumpPatternTransforms;
    //private Transform[] rightPatternTransforms;
    //private Transform[] linePatternTransforms;

    private void Awake() {
        //SetUpPatternTransforms();
    }

    public void SpawnDiscs(Lane lane, ItemType itemType) {
        GameObject patternPrefab;
        Transform tr;

        switch (lane) {
            case Lane.LEFT:
                tr = itemManager.leftLane;
                break;
            case Lane.CENTER:
                tr = itemManager.centerLane;
                break;
            case Lane.RIGHT:
                tr = itemManager.rightLane;
                break;
            default:
                tr = itemManager.leftLane;
                break;
        }

        patternPrefab = Instantiate(ChosePattern(lane, itemType), tr.position, tr.rotation, transform);

        int rand = Random.Range(0, 101);
        if (rand < platinumPatternChance) {         //1 platinum disc in the pattern
            CreateDiscPattern(patternPrefab, true);
        }
        else {
            CreateDiscPattern(patternPrefab, false);
        }
    }

    private GameObject ChosePattern(Lane lane, ItemType itemType) {
        int rand;

        if (itemType == ItemType.DISCS_ONLY) {
            rand = Random.Range(0, 2);

            if (rand == 0)
                return jumpPatternPrefab;
            else
                return linePatternPrefab;
        }
        else { //OBSTACLE_DISCS
            if (lane == Lane.LEFT) {
                rand = Random.Range(0, 2);

                if (rand == 0)
                    return jumpPatternPrefab;
                else
                    return rightPatternPrefab;
            }
            else if (lane == Lane.CENTER) {
                rand = Random.Range(0, 3);

                if (rand == 0)
                    return leftPatternPrefab;
                else if (rand == 1)
                    return jumpPatternPrefab;
                else
                    return rightPatternPrefab;
            }
            else {
                rand = Random.Range(0, 2);

                if (rand == 0)
                    return jumpPatternPrefab;
                else
                    return leftPatternPrefab;
            }
        }
    }

    //private void SetUpPatternTransforms() {
    //    InitiatePatternTransform(leftPatternTransforms, leftPatternPrefab);
    //    InitiatePatternTransform(jumpPatternTransforms, jumpPatternPrefab);
    //    InitiatePatternTransform(rightPatternTransforms, rightPatternPrefab);
    //    InitiatePatternTransform(linePatternTransforms, linePatternPrefab);
    //}

    //private void InitiatePatternTransform(Transform[] children, GameObject prefab) {
    //    children = new Transform[prefab.transform.childCount];

    //    int i = 0;
    //    foreach (Transform child in prefab.transform) {
    //        children[i] = child;
    //        i++;
    //    }
    //}

    private void CreateDiscPattern(GameObject patternPrefab, bool isPlatinum) {
        Transform[] children = new Transform[patternPrefab.transform.childCount];

        int i = 0;
        foreach (Transform child in patternPrefab.transform) {
            children[i] = child;
            i++;
        }

        if (isPlatinum) {
            int randIndex = Random.Range(0, children.Length);

            for (int j = 0; j < children.Length; j++) {
                if (j == randIndex)
                    Instantiate(platinumDiscPrefab, children[j].position, children[j].rotation, transform);
                else
                    Instantiate(goldenDiscPrefab, children[j].position, children[j].rotation, transform);
            }
        }
        else {
            foreach (Transform anchor in children) {
                Instantiate(goldenDiscPrefab, anchor.position, anchor.rotation, transform);
            }
        }
        
    }
}
