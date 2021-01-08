using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    #region Variables
    private GameManager gameManager;
    private CameraMovement cameraMovement;
    private Rigidbody rb;
    private Cop cop;
    private SpawnAlien spawnAlien;
    private ItemManager itemManager;
    private JulAnim julAnim;

    [HideInInspector]
    public Lane lane = Lane.CENTER;

    [SerializeField]
    private Transform centerPos;
    [SerializeField]
    private Transform leftPos;
    [SerializeField]
    private Transform rightPos;
    [SerializeField]
    private Transform topPos;

    [SerializeField]
    private Transform copCatchupPos;

    [SerializeField]
    private GameObject jul;
    [SerializeField]
    private GameObject twingo;
    [SerializeField]
    private GameObject tmax;
    [SerializeField]
    private GameObject ovni;

    [SerializeField]
    private float moveTime;
    [SerializeField]
    private float jumpVelocity;
    [SerializeField]
    private float jumpMultiplier;
    [SerializeField]
    private float dodgeStreakTime;
    [SerializeField]
    private float fallMultiplier;

    [HideInInspector]
    public bool isGrounded = true;
    private bool isDodgeStreak = false;
    private bool startDodgeStreakTimer = false;
    private bool isCopFollowed = false;
    [HideInInspector]
    public bool startCopFollowTimer = false;
    [HideInInspector]
    public bool isClaquettes = false;
    private bool startClaquettesTimer = false;
    [HideInInspector]
    public bool isPochon = false;
    private bool startPochonTimer = false;
    [HideInInspector]
    public bool isTwingo = false;
    private bool startTwingoTimer = false;
    [HideInInspector]
    public bool isTmax = false;
    private bool startTmaxTimer = false;
    private bool isY = false;
    private bool startYTimer = false;
    [HideInInspector]
    public bool isTmaxFlying = false;
    [HideInInspector]
    public bool isOvni = false;
    private bool startOvniTimer = false;
    private bool isStrafing = false;

    private float dodgeStreakTimer;
    private float copFollowTimer;
    private float claquettesTimer;
    private float pochonTimer;
    private float twingoTimer;
    private float tmaxTimer;
    private float yTimer;
    private float ovniTimer;

    private const float COP_FOLLOW_DURATION = 5f;

    [Header("Bonus variables")]
    [SerializeField]
    private float claquettesJumpVelocity;
    [SerializeField]
    private float claquettesDuration;
    [SerializeField]
    private float pochonDuration;
    [SerializeField]
    private float twingoSpeed;
    [SerializeField]
    private float twingoDuration;
    [SerializeField]
    private float tmaxSpeed;
    public float tmaxDuration;
    [SerializeField]
    private float alienSpawnRate;
    [SerializeField]
    private float ySpeed;
    [SerializeField]
    private float yDuration;
    //[SerializeField]
    //private float ovniSpeed;
    [SerializeField]
    private float ovniDuration;
    [SerializeField]
    private float ovniSpeed;
    #endregion

    #region MonoBehaviour
    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();
        rb = GetComponent<Rigidbody>();
        cameraMovement = FindObjectOfType<CameraMovement>();
        cop = FindObjectOfType<Cop>();
        spawnAlien = FindObjectOfType<SpawnAlien>();
        itemManager = FindObjectOfType<ItemManager>();
        julAnim = FindObjectOfType<JulAnim>();

        GameEvents.current.onReplayButtonTrigger += OnReplay;
        GameEvents.current.onMainMenuButtonTrigger += OnReplay;
    }

    private void Update() {
        DodgeStreakTimerUpdate();  //dodge streak timer
        ClaquettesTimerUpdate();   //claquettes timer
        PochonTimerUpdate();        //pochon timer
        TwingoTimerUpdate();        //twingo timer
        CopFollowTimerUpdate();     //cop timer
        TmaxTimerUpdate();          //tmax timer
        YTimerUpdate();             //mise en y timer
        OvniTimerUpdate();          //ovni timer

        if (rb.velocity.y < 0) {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0) {
            rb.velocity += Vector3.up * Physics.gravity.y * (jumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private void OnDestroy() {
        GameEvents.current.onReplayButtonTrigger -= OnReplay;
        GameEvents.current.onMainMenuButtonTrigger -= OnReplay;
    }
    #endregion

    #region Methods
    private void OnReplay() {
        transform.position = centerPos.position;
        //julanimator settrigger pour renvoyer au state init
        lane = Lane.CENTER;

        isGrounded = true;
        isDodgeStreak = false;
        startDodgeStreakTimer = false;
        isCopFollowed = false;
        startCopFollowTimer = false;
        isClaquettes = false;
        startClaquettesTimer = false;
        isPochon = false;
        startPochonTimer = false;
        isTwingo = false;
        startTwingoTimer = false;
        isTmax = false;
        startTmaxTimer = false;
        isY = false;
        startYTimer = false;
        isTmaxFlying = false;
        isOvni = false;
        startOvniTimer = false;
        isStrafing = false;

        ActivateLook(jul);
    }

    public void MoveToLeft() {
        if (lane == Lane.RIGHT) {
            lane = Lane.CENTER;
            isStrafing = true;
            transform.DOMoveX(centerPos.position.x, moveTime).SetEase(Ease.Linear).OnComplete(() => StrafeComplete());
            copCatchupPos.DOMoveX(centerPos.position.x, moveTime).SetEase(Ease.Linear);
            cameraMovement.MoveToCenterPos(moveTime);

            if (isGrounded) {
                julAnim.Strafe();
                cop.Strafe();
            }
        }
        else if (lane == Lane.CENTER) {
            lane = Lane.LEFT;
            isStrafing = true;
            transform.DOMoveX(leftPos.position.x, moveTime).SetEase(Ease.Linear).OnComplete(() => StrafeComplete());
            copCatchupPos.DOMoveX(leftPos.position.x, moveTime).SetEase(Ease.Linear);
            cameraMovement.MoveToLeftPos(moveTime);

            if (isGrounded) {
                julAnim.Strafe();
                cop.Strafe();
            }
        }
        else {
            if (!(isTwingo || isTmax || isOvni)) {
                Time.timeScale = 1f;        //make function to manage slow when hitting obstacle
                if (isCopFollowed) {
                    gameManager.Lose();
                }
                else {
                    startCopFollowTimer = true;
                    cop.CatchUpToPlayer();
                }
            }
        }
    }

    public void MoveToRight() {
        if (lane == Lane.LEFT) {
            lane = Lane.CENTER;
            isStrafing = true;
            transform.DOMoveX(centerPos.position.x, moveTime).SetEase(Ease.Linear).OnComplete(() => StrafeComplete());
            copCatchupPos.DOMoveX(centerPos.position.x, moveTime).SetEase(Ease.Linear);
            cameraMovement.MoveToCenterPos(moveTime);

            if (isGrounded) {
                julAnim.Strafe();
                cop.Strafe();
            }
        }
        else if (lane == Lane.CENTER) {
            lane = Lane.RIGHT;
            isStrafing = true;
            transform.DOMoveX(rightPos.position.x, moveTime).SetEase(Ease.Linear).OnComplete(() => StrafeComplete());
            copCatchupPos.DOMoveX(rightPos.position.x, moveTime).SetEase(Ease.Linear);
            cameraMovement.MoveToRightPos(moveTime);

            if (isGrounded) {
                julAnim.Strafe();
                cop.Strafe();
            }
        }
        else {
            Time.timeScale = 1f;        //make function to manage slow when hitting obstacle
            if (isCopFollowed) {
                gameManager.Lose();
            }
            else {
                startCopFollowTimer = true;
                cop.CatchUpToPlayer();
            }
        }
    }

    private void StrafeComplete() {
        rb.velocity = new Vector3(0, rb.velocity.y);
        isStrafing = false;
    }

    public void Fly() {
        if (!isTmaxFlying) {
            transform.DOMoveY(topPos.position.y, cameraMovement.flyMoveTime);
            cameraMovement.MoveToTopPos(); //moveTime * 2f

            isTmaxFlying = true;
            rb.useGravity = false;

            itemManager.StartSpawnOvniDiscs();
        }

        startTmaxTimer = true;

        float spawnTime = Random.Range(0, tmaxDuration - 4f);
        Invoke("SpawnFlyingAlien", spawnTime);
    }

    public void EndFly() {
        if (isTmaxFlying)
            tmaxTimer = 0f;

        rb.useGravity = true;

        //transform.DOMoveY(centerPos.position.y, moveTime * 2f);
        cameraMovement.MoveToGroundPos();  //moveTime * 4f
        cameraMovement.MoveToNormalPosZ(); //moveTime * 4f
        Debug.Log("end fly");
    }

    public void Jump() {
        if (isGrounded) {
            if (isTmax) {
                if (!isY) {
                    startYTimer = true;
                    Time.timeScale += ySpeed;
                    tmax.transform.DOLocalRotate(new Vector3(-55f, 0f, 0f), 0.2f);
                }
            }
            else if (isTwingo) return;
            else {
                isGrounded = false;
                julAnim.Jump();
                cop.Jump();

                float dur = 0.9f;
                copCatchupPos.DOMoveY(3f, dur / 2f).SetEase(Ease.Linear).OnComplete(() => copCatchupPos.DOMoveY(0, dur / 2f).SetEase(Ease.Linear));

                //Sequence sequence = DOTween.Sequence();

                //sequence.Append(rb.DOMoveY(transform.position.y + 3, jumpTime/2).SetEase(Ease.OutSine));
                ////sequence.Append(transform.DOMoveY(transform.position.y, jumpTime/2).SetEase(Ease.InSine));
                //sequence.OnComplete(() => isGrounded = true);

                //sequence.Play();

                if (isClaquettes) {
                    rb.velocity = Vector3.up * claquettesJumpVelocity;
                }
                else {
                    rb.velocity = Vector3.up * jumpVelocity;
                }
            }
        }
    }

    public void HitByObstacle(Collider col) {
        Obstacles obstacle = col.gameObject.GetComponent<Obstacles>();

        if (col.tag == "Camionette") {
            Time.timeScale = 1;
            if (isStrafing && lane != obstacle.currentLane) {
                if (isCopFollowed) {
                    gameManager.Lose();
                }
                else {
                    startCopFollowTimer = true;
                    // move the cops in fov
                    cop.CatchUpToPlayer();
                }
            }
            else {
                gameManager.Lose();
            }
        }

        if (isTwingo) {
            if (col.tag == "Barriere" || col.tag == "Plot" || col.tag == "Rat") {
                // break them
            }
            else if (col.tag == "Voiture") {
                Time.timeScale = 1;
                gameManager.Lose();
            }
        }
        else if (isTmax) {
            if (isY) {
                if (col.tag == "Barriere" || col.tag == "Plot" || col.tag == "Rat" || col.tag == "Voiture") {
                    // break them
                }
            }
            else {
                if (col.tag == "Barriere" || col.tag == "Plot" || col.tag == "Rat") {
                    // break them
                }
                else if (col.tag == "Voiture") {
                    Time.timeScale = 1;
                    gameManager.Lose();
                }
            }
        }
        else {
            Time.timeScale = 1;
            if (col.tag == "Barriere" || col.tag == "Plot" || col.tag == "Rat") {   //obstacles légers a pied
                if (isCopFollowed) {
                    gameManager.Lose();
                }
                else {
                    startCopFollowTimer = true;
                    cop.CatchUpToPlayer();
                }
            }
            else if (col.tag == "Voiture") {
                if (isStrafing && lane != obstacle.currentLane) {
                    if (isCopFollowed) {
                        gameManager.Lose();
                    }
                    else {
                        startCopFollowTimer = true;
                        cop.CatchUpToPlayer();
                    }
                }
                else {
                    gameManager.Lose();
                }
            }
        }

        Destroy(col.gameObject);
    }

    public void HitByBonus(string tag) {
        Debug.Log("hit by bonus");
        switch (tag) {
            case "Claquettes":
                startClaquettesTimer = true;
                break;
            case "Pochon":
                startPochonTimer = true;
                //if (!isPochon)
                //    Time.timeScale += pochonSpeed;
                break;
            case "Twingo":
                if (isClaquettes) claquettesTimer = 0f;

                copFollowTimer = 0f;
                startTwingoTimer = true;
                ActivateLook(twingo);

                if (!isTwingo)
                    Time.timeScale += twingoSpeed;
                break;
            case "Tmax":
                if (isClaquettes) claquettesTimer = 0f;

                copFollowTimer = 0f;
                startTmaxTimer = true;
                ActivateLook(tmax);

                if (!isTmax) {
                    Time.timeScale += tmaxSpeed;
                    AlienEvent();
                }

                break;
            case "Ovni":
                break;
            default:
                break;
        }
    }

    public void StartOvni() {
        copFollowTimer = 0f;
        startOvniTimer = true;
        ActivateLook(ovni);
    }

    public void HitByDisc(bool isGold) {
        gameManager.AddDiscScore(isGold);
    }

    public void DodgeObstacle() {
        startDodgeStreakTimer = true;

        gameManager.AddDodgeScore(isDodgeStreak);
    }

    private void AlienEvent() {
        int rand = Random.Range(1, 101);

        if (rand <= alienSpawnRate) {
            spawnAlien.StartAlienSpawn();
        }
    }

    private void SpawnFlyingAlien() {
        spawnAlien.SpawnNextAlien();
    }

    private void ActivateLook(GameObject go) {
        jul.SetActive(false);
        twingo.SetActive(false);
        tmax.SetActive(false);
        ovni.SetActive(false);

        go.SetActive(true);
    }

    private void DodgeStreakTimerUpdate() {
        if (startDodgeStreakTimer) {
            isDodgeStreak = true;
            dodgeStreakTimer = dodgeStreakTime;

            startDodgeStreakTimer = false;
        }

        if (isDodgeStreak) {
            if (dodgeStreakTimer <= 0f) {
                isDodgeStreak = false;
            }
            else {
                if (gameManager.gameState != GameState.PAUSE) dodgeStreakTimer -= Time.unscaledDeltaTime;
            }
        }
    }

    private void CopFollowTimerUpdate() {
        if (startCopFollowTimer) {
            isCopFollowed = true;
            copFollowTimer = COP_FOLLOW_DURATION;

            startCopFollowTimer = false;
        }

        if (isCopFollowed) {
            if (copFollowTimer <= 0f) {
                isCopFollowed = false;
                cop.GoBackToInitialPos();
            }
            else {
                if (gameManager.gameState != GameState.PAUSE) copFollowTimer -= Time.unscaledDeltaTime;
            }
        }
    }

    private void ClaquettesTimerUpdate() {
        if (startClaquettesTimer) {
            isClaquettes = true;
            claquettesTimer = claquettesDuration;

            startClaquettesTimer = false;
        }

        if (isClaquettes) {
            if (claquettesTimer <= 0f) {
                isClaquettes = false;
            }
            else {
                if (gameManager.gameState != GameState.PAUSE) claquettesTimer -= Time.unscaledDeltaTime;
            }
        }
    }

    private void PochonTimerUpdate() {
        if (startPochonTimer) {
            isPochon = true;
            pochonTimer = pochonDuration;

            startPochonTimer = false;
        }

        if (isPochon) {
            if (pochonTimer <= 0f) {
                isPochon = false;
                //Time.timeScale -= pochonSpeed;
            }
            else {
                if (gameManager.gameState != GameState.PAUSE) pochonTimer -= Time.unscaledDeltaTime;
            }
        }
    }

    private void TwingoTimerUpdate() {
        if (startTwingoTimer) {
            isTwingo = true;
            twingoTimer = twingoDuration;

            startTwingoTimer = false;
        }

        if (isTwingo) {
            if (twingoTimer <= 0f) {
                isTwingo = false;
                ActivateLook(jul);

                Time.timeScale -= twingoSpeed;
            }
            else {
                if (gameManager.gameState != GameState.PAUSE) twingoTimer -= Time.unscaledDeltaTime;
            }
        }
    }

    private void TmaxTimerUpdate() {
        if (startTmaxTimer) {
            isTmax = true;
            tmaxTimer = tmaxDuration;

            startTmaxTimer = false;
        }

        if (isTmax) {
            if (tmaxTimer <= 0f) {
                if (!isOvni) {
                    isTmax = false;
                    yTimer = 0f;
                    ActivateLook(jul);

                    Time.timeScale -= tmaxSpeed;
                }
            }
            else {
                if (gameManager.gameState != GameState.PAUSE) tmaxTimer -= Time.unscaledDeltaTime;
            }
        }
    }

    private void YTimerUpdate() {
        if (startYTimer) {
            isY = true;
            yTimer = yDuration;

            startYTimer = false;
        }

        if (isY) {
            if (yTimer <= 0f) {
                isY = false;
                tmax.transform.DOLocalRotate(new Vector3(0f, 0f, 0f), 0.2f);
                Time.timeScale -= ySpeed;
            }
            else {
                if (gameManager.gameState != GameState.PAUSE) yTimer -= Time.unscaledDeltaTime;
            }
        }
    }

    private void OvniTimerUpdate() {
        if (startOvniTimer) {
            isOvni = true;
            isTmax = false;
            ovniTimer = ovniDuration;

            Time.timeScale -= tmaxSpeed;
            Time.timeScale += ovniSpeed;

            cameraMovement.MoveToOvniPosY();   //moveTime * 2f
            cameraMovement.MoveToOvniPosZ();   //moveTime * 2f

            startOvniTimer = false;
        }

        if (isOvni) {
            if (ovniTimer <= 0f) {
                isOvni = false;
                ActivateLook(jul);

                Time.timeScale -= ovniSpeed;

                EndFly();
            }
            else {
                if (gameManager.gameState != GameState.PAUSE) ovniTimer -= Time.unscaledDeltaTime;
            }
        }
    }

    public void StartAnimation() {
        julAnim.StartAnimation();
    }
    #endregion
}
