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
    private GameObject leftTurnPatternPrefab;
    [SerializeField]
    private GameObject jumpPatternPrefab;
    [SerializeField]
    private GameObject rightPatternPrefab;
    [SerializeField]
    private GameObject rightTurnPatternPrefab;
    [SerializeField]
    private GameObject linePatternPrefab;
    [SerializeField]
    private GameObject claquettesJumpPatternPrefab;
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
        player = FindObjectOfType<Player>();
        gameManager = FindObjectOfType<GameManager>();
    }

    public GameObject SpawnDiscs(Lane lane, GameObject obstacle = null) {   //return pattern prefab
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
        return patternPrefab;
    }

    private void RandPlatinumDiscPattern(GameObject patternPrefab, bool isOvniPattern = false, bool isLastOvniPattern = false) {    //apply normal or platinum discs to a pattern
        int rand = Random.Range(0, 101);
        if (rand < platinumPatternChance) {         //1 platinum disc in the pattern
            CreateDiscPattern(patternPrefab, true, isOvniPattern, isLastOvniPattern);
        }
        else {
            CreateDiscPattern(patternPrefab, false, isOvniPattern, isLastOvniPattern);
        }
    }

    private GameObject ChosePattern(Lane lane, GameObject obstacle) {   //patern for obstacles
        int rand;

        if (lane == Lane.LEFT) {
            rand = Random.Range(0, 2);
            if (obstacle.tag == "Voiture" || obstacle.tag == "Camionette") {
                if (player.isClaquettes || gameManager.isClaquetteInGame) {  //can spawn jump pattern if player isclaquettes and it's a vehicle
                    if (rand == 0)
                        return claquettesJumpPatternPrefab;
                    else
                        return RandSidePatterns(false);
                }
                else
                    return RandSidePatterns(false);
            }
            else {
                if (player.isTmax || gameManager.isTmaxInGame || player.isTwingo || gameManager.isTwingoInGame)
                    return RandSidePatterns(false);
                else {
                    if (rand == 0) {
                        if (player.isClaquettes || gameManager.isClaquetteInGame) return claquettesJumpPatternPrefab;
                        else return jumpPatternPrefab;
                    }
                    else
                        return RandSidePatterns(false);
                }
            }
        }
        else if (lane == Lane.CENTER) {
            rand = Random.Range(0, 3);
            if (obstacle.tag == "Voiture" || obstacle.tag == "Camionette") {
                if (player.isClaquettes || gameManager.isClaquetteInGame) {
                    if (rand == 0)
                        return RandSidePatterns(true);
                    else if (rand == 1)
                        return claquettesJumpPatternPrefab;
                    else
                        return RandSidePatterns(false);
                }
                else {
                    rand = Random.Range(0, 2);
                    if (rand == 0)
                        return RandSidePatterns(true);
                    else
                        return RandSidePatterns(false);
                }
            }
            else {
                if (player.isTmax || gameManager.isTmaxInGame || player.isTwingo || gameManager.isTwingoInGame) {
                    rand = Random.Range(0, 2);
                    if (rand == 0)
                        return RandSidePatterns(true);
                    else
                        return RandSidePatterns(false);
                }
                else {
                    if (rand == 0)
                        return RandSidePatterns(true);
                    else if (rand == 1)
                        if (player.isClaquettes || gameManager.isClaquetteInGame) return claquettesJumpPatternPrefab;
                        else return jumpPatternPrefab;
                    else
                        return RandSidePatterns(false);
                }
            }
        }
        else {
            rand = Random.Range(0, 2);
            if (obstacle.tag == "Voiture" || obstacle.tag == "Camionette") {
                if (player.isClaquettes || gameManager.isClaquetteInGame) {  //can spawn jump pattern if player isclaquettes and it's a vehicle
                    if (rand == 0)
                        return claquettesJumpPatternPrefab;
                    else
                        return RandSidePatterns(true);
                }
                else {
                    return RandSidePatterns(true);
                }
            }
            else {
                if (player.isTmax || gameManager.isTmaxInGame || player.isTwingo || gameManager.isTwingoInGame) {
                    return RandSidePatterns(true);
                }
                else {
                    if (rand == 0)
                        if (player.isClaquettes || gameManager.isClaquetteInGame) return claquettesJumpPatternPrefab;
                        else return jumpPatternPrefab;
                    else
                        return RandSidePatterns(true);
                }
            }
        }
    }

    private GameObject ChosePattern() { //pattern for only discs
        int rand = Random.Range(0, 2);

        if (player.isTmax || gameManager.isTmaxInGame || player.isTwingo || gameManager.isTwingoInGame) {
            return linePatternPrefab;
        }
        else {
            if (rand == 0)
                if (player.isClaquettes || gameManager.isClaquetteInGame) return claquettesJumpPatternPrefab;
                else return jumpPatternPrefab;
            else
                return linePatternPrefab;
        }
    }

    private GameObject RandSidePatterns(bool isLeft) {
        int rand = Random.Range(0, 2);

        if (isLeft) {
            if (rand == 0) return leftPatternPrefab;
            else return leftTurnPatternPrefab;
        }
        else {
            if (rand == 0) return rightPatternPrefab;
            else return rightTurnPatternPrefab;
        }
    }

    private void CreateDiscPattern(GameObject patternPrefab, bool isPlatinum, bool isOvniPattern = false, bool isLastOvniPattern = false) { //apply discs on pattern
        Transform[] children = new Transform[patternPrefab.transform.childCount];

        int i = 0;
        foreach (Transform child in patternPrefab.transform) {
            children[i] = child;
            i++;
        }

        if (isPlatinum) {
            int randIndex = Random.Range(0, children.Length);

            for (int j = 0; j < children.Length; j++) {
                GameObject disc;
                if (j == randIndex)
                    disc = Instantiate(platinumDiscPrefab, children[j].position, children[j].rotation, transform);
                else
                    //disc = Instantiate(goldenDiscPrefab, children[j].position, children[j].rotation, transform);
                    disc = ObjectPooler.current.InstantiateObject(children[j].position, children[j].rotation, transform);

                if (isOvniPattern) {
                    disc.GetComponent<Disc>().isOvniDisc = true;

                    if (isLastOvniPattern) {
                        if (j == children.Length - 1) disc.GetComponent<Disc>().isLastOvniDisc = true;
                    }
                }
            }
        }
        else {
            //foreach (Transform anchor in children) {
            //    Instantiate(goldenDiscPrefab, anchor.position, anchor.rotation, transform);
            //}

            for (int j = 0; j < children.Length; j++) {
                GameObject disc = ObjectPooler.current.InstantiateObject(children[j].position, children[j].rotation, transform);

                if (isOvniPattern) {
                    disc.GetComponent<Disc>().isOvniDisc = true;

                    if (isLastOvniPattern) {
                        if (j == children.Length - 1) disc.GetComponent<Disc>().isLastOvniDisc = true;
                    }
                }
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
        for (int i = 1; i < laneArray.Length; i++) {
            GameObject igLinePattern =
                Instantiate(ovniLinePattern,
                GetLastOvniDiscPos(igPatterns[igPatterns.Count - 1], offSet),
                tr.rotation,
                transform);
            igPatterns.Add(igLinePattern);
            // add turn pattern at the end of linepattern
            if (i < laneArray.Length - 1) { //pour voir l'avant derniere et la derniere lane
                GameObject turnPattern = GetTurnPattern(laneArray[i], laneArray[i + 1]);
                if (turnPattern != null) {   //si il y a un turn a faire
                    GameObject igTurnPattern = Instantiate(turnPattern, GetLastOvniDiscPos(igLinePattern, 2), tr.rotation, transform);
                    igPatterns.Add(igTurnPattern);
                }
            }
        }
        #endregion

        //apply discs to pattern
        //foreach (GameObject pattern in igPatterns) {
        //    Debug.Log(pattern);
        //    RandPlatinumDiscPattern(pattern);
        //}

        for (int i = 0; i < igPatterns.Count; i++) {
            if (i == igPatterns.Count - 1) RandPlatinumDiscPattern(igPatterns[i], true, true);
            else RandPlatinumDiscPattern(igPatterns[i], true);
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

    private IEnumerator InstantiateOvniDiscs(bool isPlatinum, GameObject patternPrefab, bool isLastOvniPattern) {
        //float instantiateTime = 0.1f;
        Transform[] children = new Transform[patternPrefab.transform.childCount];

        int i = 0;
        foreach (Transform child in patternPrefab.transform) {
            children[i] = child;
            i++;
        }

        if (isPlatinum) {
            int randIndex = Random.Range(0, children.Length);

            for (int j = 0; j < children.Length; j++) {
                GameObject disc;
                if (j == randIndex)
                    disc = Instantiate(platinumDiscPrefab, children[j].position, children[j].rotation, transform);
                else
                    disc = Instantiate(goldenDiscPrefab, children[j].position, children[j].rotation, transform);

                disc.GetComponent<Disc>().isOvniDisc = true;

                if (isLastOvniPattern) {
                    if (j == children.Length - 1) disc.GetComponent<Disc>().isLastOvniDisc = true;
                }

                yield return new WaitForEndOfFrame();
            }
        }
        else {
            for (int j = 0; j < children.Length; j++) {
                GameObject disc = Instantiate(goldenDiscPrefab, children[j].position, children[j].rotation, transform);

                disc.GetComponent<Disc>().isOvniDisc = true;

                if (isLastOvniPattern) {
                    if (j == children.Length - 1) disc.GetComponent<Disc>().isLastOvniDisc = true;
                }

                yield return new WaitForEndOfFrame();
            }
        }
    }

    public void SpawnSecondDiscs(GameObject firstObstacle, Lane[] availableLanes) {
        // bool isrightavailable, isleftavailable, iscenteravailable en parcourant le tableau
    }
}
