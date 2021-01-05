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
    [Header("Ovni discs")]
    [SerializeField]
    private GameObject ovniLinePattern;
    [SerializeField]
    private GameObject turnLeftPattern;
    [SerializeField]
    private GameObject turnRightPattern;
    [SerializeField]
    [Min(2)]
    private int nbLines;

    private Player player;
    private GameManager gameManager;

    private void Awake() {
        //SetUpPatternTransforms();
        player = FindObjectOfType<Player>();
        gameManager = FindObjectOfType<GameManager>();
    }

    public void SpawnDiscs(Lane lane, GameObject obstacle = null) {
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

        if (obstacle != null)
            patternPrefab = Instantiate(ChosePattern(lane, obstacle), tr.position, tr.rotation, transform);
        else
            patternPrefab = Instantiate(ChosePattern(), tr.position, tr.rotation, transform);

        RandPlatinumDiscPattern(patternPrefab);
    }

    private void RandPlatinumDiscPattern(GameObject patternPrefab) {    //apply normal or platinum discs to a pattern
        int rand = Random.Range(0, 101);
        if (rand < platinumPatternChance) {         //1 platinum disc in the pattern
            CreateDiscPattern(patternPrefab, true);
        }
        else {
            CreateDiscPattern(patternPrefab, false);
        }
    }

    private GameObject ChosePattern(Lane lane, GameObject obstacle) {   //patern for obstacles
        int rand;

        if (lane == Lane.LEFT) {
            rand = Random.Range(0, 2);
            if (obstacle.tag == "Voiture" || obstacle.tag == "Camionette") {
                if (player.isClaquettes) {  //can spawn jump pattern if player isclaquettes and it's a vehicle
                    if (rand == 0)
                        return jumpPatternPrefab;
                    else
                        return rightPatternPrefab;
                }
                else {
                    return rightPatternPrefab;
                }
            }
            else {
                if (player.isTmax || gameManager.isTmaxInGame || player.isTwingo || gameManager.isTwingoInGame) {
                    return rightPatternPrefab;
                }
                else {
                    if (rand == 0)
                        return jumpPatternPrefab;
                    else
                        return rightPatternPrefab;
                }
            }
        }
        else if (lane == Lane.CENTER) {
            rand = Random.Range(0, 3);
            if (obstacle.tag == "Voiture" || obstacle.tag == "Camionette") {
                if (player.isClaquettes) {
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
                        return leftPatternPrefab;
                    else
                        return rightPatternPrefab;
                }
            }
            else {
                if (player.isTmax || gameManager.isTmaxInGame || player.isTwingo || gameManager.isTwingoInGame) {
                    rand = Random.Range(0, 2);
                    if (rand == 0)
                        return leftPatternPrefab;
                    else
                        return rightPatternPrefab;
                }
                else {
                    if (rand == 0)
                        return leftPatternPrefab;
                    else if (rand == 1)
                        return jumpPatternPrefab;
                    else
                        return rightPatternPrefab;
                }
            }
        }
        else {
            rand = Random.Range(0, 2);
            if (obstacle.tag == "Voiture" || obstacle.tag == "Camionette") {
                if (player.isClaquettes) {  //can spawn jump pattern if player isclaquettes and it's a vehicle
                    if (rand == 0)
                        return jumpPatternPrefab;
                    else
                        return leftPatternPrefab;
                }
                else {
                    return leftPatternPrefab;
                }
            }
            else {
                if (player.isTmax || gameManager.isTmaxInGame || player.isTwingo || gameManager.isTwingoInGame) {
                    return leftPatternPrefab;
                }
                else {
                    if (rand == 0)
                        return jumpPatternPrefab;
                    else
                        return leftPatternPrefab;
                }
            }
        }
    }

    private GameObject ChosePattern() { //pattern for only discs
        int rand= Random.Range(0, 2);

        if (player.isTmax || gameManager.isTmaxInGame || player.isTwingo || gameManager.isTwingoInGame) {
            return linePatternPrefab;
        }
        else {
            if (rand == 0)
                return jumpPatternPrefab;
            else
                return linePatternPrefab;
        }
    }

    private void CreateDiscPattern(GameObject patternPrefab, bool isPlatinum) { //apply discs on pattern
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

    public void SpawnOvniDiscs() {
        //create random overall pattern
        //create array random Lane
        Lane[] laneArray = new Lane[nbLines];

        laneArray[0] = GetRandomLane(Lane.CENTER);
        for (int i = 1; i < laneArray.Length; i++) {
            laneArray[i] = GetRandomLane(laneArray[i - 1]);
        }
        //create array patterns
        List<GameObject> igPatterns = new List<GameObject>();
        //instantiate first line - fonction init ovni disc pattern
        #region Init
        Transform tr;
        if (laneArray[0] == Lane.LEFT) tr = itemManager.leftLane;
        else if (laneArray[0] == Lane.CENTER) tr = itemManager.centerLane;
        else tr = itemManager.rightLane;

        GameObject firstIgLinePattern = Instantiate(ovniLinePattern, new Vector3(tr.position.x, itemManager.topPos.position.y, tr.position.z), tr.rotation, transform);
        igPatterns.Add(firstIgLinePattern);
        int offSet = 2;
        GameObject firstTurnPattern = GetTurnPattern(laneArray[0], laneArray[1]);
        if (firstTurnPattern != null) {   //si il y a un turn a faire
            GameObject firstIgTurnPattern = Instantiate(firstTurnPattern, GetLastOvniDiscPos(firstIgLinePattern, offSet), tr.rotation, transform);
            igPatterns.Add(firstIgTurnPattern);
        }
        #endregion

        #region LoopTheRest
        for (int i = 1; i < laneArray.Length - 1; i++) {
            GameObject igLinePattern =
                Instantiate(ovniLinePattern,
                GetLastOvniDiscPos(igPatterns[igPatterns.Count - 1], offSet),
                tr.rotation,
                transform);
            igPatterns.Add(igLinePattern);
            // add turn pattern at the end of linepattern
            if (i < laneArray.Length - 2) { //pour voir l'avant derniere et la derniere lane
                GameObject turnPattern = GetTurnPattern(laneArray[i], laneArray[i + 1]);
                if (turnPattern != null) {   //si il y a un turn a faire
                    GameObject igTurnPattern = Instantiate(turnPattern, GetLastOvniDiscPos(igLinePattern, 2), tr.rotation, transform);
                    igPatterns.Add(igTurnPattern);
                }
            }
        }
        #endregion

        //apply discs to pattern
        foreach (GameObject pattern in igPatterns) {
            Debug.Log(pattern);
            RandPlatinumDiscPattern(pattern);
        }
    }

    private Lane GetRandomLane(Lane previousLane) {
        int rand;

        switch (previousLane) {
            case Lane.LEFT:
                rand = Random.Range(0, 2);

                if (rand == 0) return Lane.LEFT;
                else return Lane.CENTER;
                break;

            case Lane.CENTER:
                rand = Random.Range(0, 3);

                if (rand == 0) return Lane.LEFT;
                else if (rand == 1) return Lane.CENTER;
                else return Lane.RIGHT;
                break;

            case Lane.RIGHT:
                rand = Random.Range(0, 2);

                if (rand == 0) return Lane.RIGHT;
                else return Lane.CENTER;
                break;

            default:
                rand = Random.Range(0, 3);

                if (rand == 0) return Lane.LEFT;
                else if (rand == 1) return Lane.CENTER;
                else return Lane.RIGHT;
                break;
        }
    }

    private GameObject GetTurnPattern(Lane currentLane, Lane nextLane) {    //return a turn pattern if needed, return null else
        if (currentLane == Lane.LEFT) {
            if (nextLane == Lane.CENTER) return turnRightPattern;
        }
        else if (currentLane == Lane.CENTER) {
            if (nextLane == Lane.LEFT) return turnLeftPattern;
            else if (nextLane == Lane.RIGHT) return turnRightPattern;
        }
        else if (currentLane == Lane.RIGHT) {
            if (nextLane == Lane.CENTER) return turnLeftPattern;
        }

        return null;
    }

    private Vector3 GetLastOvniDiscPos(GameObject pattern, int offSet) {    //return pos of the last disc pos of the pattern with an offset
        Vector3 lastChild = pattern.transform.GetChild(pattern.transform.childCount - 1).position;

        return new Vector3(lastChild.x, lastChild.y, lastChild.z + offSet);
    }
}
