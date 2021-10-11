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
    private CapsuleCollider julCollider;
    private BoxCollider twingoOvniCollider;
    private Transform playerTransform = null;

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
    private Vector3 initTwingoPos;

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

    [SerializeField]
    private Animator twingoAnimator;
    [SerializeField]
    private Animator tmaxAnimator;
    [SerializeField]
    private Animator ovniAnimator;

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

    [SerializeField] private TmaxLose tmaxLose;
    [SerializeField] private Parallaxe parallaxe;
    [SerializeField] private VFXManager vfxManager;
    [SerializeField] private float obstacleDetectionRadius = 0f;

    [Header("Bonus variables")]
    [SerializeField]
    private float claquettesJumpVelocity;
    [SerializeField]
    private float claquettesDuration;
    [SerializeField]
    private float pochonDuration;
    public float twingoSpeed;
    [SerializeField]
    private float twingoDuration;
    public float tmaxSpeed;
    public float tmaxDuration;
    [SerializeField]
    private float alienSpawnRate;
    public float ySpeed;
    [SerializeField]
    private float yDuration;
    //[SerializeField]
    //private float ovniDuration;
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
        julCollider = GetComponent<CapsuleCollider>();
        twingoOvniCollider = GetComponent<BoxCollider>();
        playerTransform = transform;

        GameEvents.current.onReplayButtonTrigger += OnReplay;
        GameEvents.current.onMainMenuButtonTrigger += OnReplay;
        GameEvents.current.onPreContinueGame += OnPreContinue;

        initTwingoPos = twingo.transform.position;
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

        if (!(isTmaxFlying || isOvni)) {
            if (rb.velocity.y < 0) {
                rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (rb.velocity.y > 0) {
                rb.velocity += Vector3.up * Physics.gravity.y * (jumpMultiplier - 1) * Time.deltaTime;
            }
        }
    }

    private void OnDestroy() {
        GameEvents.current.onReplayButtonTrigger -= OnReplay;
        GameEvents.current.onMainMenuButtonTrigger -= OnReplay;
        GameEvents.current.onPreContinueGame -= OnPreContinue;
    }
    #endregion

    #region Methods
    private void OnReplay() {
        playerTransform.position = centerPos.position;
        lane = Lane.CENTER;

        ResetJul();
        ResetTwingo();
    }

    private void ResetJul()
    {
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
        rb.useGravity = true;

        ActivateLook(jul);
        twingoOvniCollider.enabled = false;
        julCollider.enabled = true;
        vfxManager.SetActiveVfxLoseOnFoot(false);
        vfxManager.SetActiveVfxWeed(false);
        vfxManager.SetActiveVfxShoes(false);
    }

    private void OnPreContinue()
    {
        ReplacePlayer();
        ResetJul();
        DeleteNearbyObstacles();
    }

    private void DeleteNearbyObstacles()
    {
        Collider[] obstacles = Physics.OverlapSphere(centerPos.position, obstacleDetectionRadius, 1 << 8);

        foreach (Collider obstacleCol in obstacles)
        {
            Destroy(obstacleCol.gameObject);
        }
    }

    private void ReplacePlayer()
    {
        switch (lane)
        {
            case Lane.LEFT:
                playerTransform.position = leftPos.position;
                break;
            case Lane.CENTER:
                playerTransform.position = centerPos.position;
                break;
            case Lane.RIGHT:
                playerTransform.position = rightPos.position;
                break;
            default:
                break;
        }
    }

    private void ResetTwingo()
    {
        twingo.transform.position = initTwingoPos;
        twingo.transform.rotation = Quaternion.identity;
    }

    public void MoveToLeft() {
        if (lane == Lane.RIGHT) {
            lane = Lane.CENTER;
            isStrafing = true;
            playerTransform.DOMoveX(centerPos.position.x, moveTime).SetEase(Ease.Linear).OnComplete(() => StrafeComplete());
            copCatchupPos.DOMoveX(centerPos.position.x, moveTime).SetEase(Ease.Linear);
            cameraMovement.MoveToCenterPos(moveTime);
            parallaxe.MoveLeft(moveTime);

            if (isGrounded) {
                if (isTwingo) {
                    twingoAnimator.SetTrigger("strafeLeftTrigger");
                }
                else if (isTmax) {
                    tmaxAnimator.SetTrigger("strafeLeftTrigger");
                }
                else {
                    julAnim.Strafe();
                    cop.Strafe();
                }
            }
            else {
                if (isOvni) {
                    ovniAnimator.SetTrigger("strafeLeftTrigger");
                }
                else if (isTmaxFlying) {
                    tmaxAnimator.SetTrigger("strafeLeftTrigger");
                }
            }

            SoundManager.current.PlaySound(SoundType.STRAFE);
        }
        else if (lane == Lane.CENTER) {
            lane = Lane.LEFT;
            isStrafing = true;
            playerTransform.DOMoveX(leftPos.position.x, moveTime).SetEase(Ease.Linear).OnComplete(() => StrafeComplete());
            copCatchupPos.DOMoveX(leftPos.position.x, moveTime).SetEase(Ease.Linear);
            cameraMovement.MoveToLeftPos(moveTime);
            parallaxe.MoveLeft(moveTime);

            if (isGrounded) {
                if (isTwingo) {
                    twingoAnimator.SetTrigger("strafeLeftTrigger");
                }
                else if (isTmax) {
                    tmaxAnimator.SetTrigger("strafeLeftTrigger");
                }
                else {
                    julAnim.Strafe();
                    cop.Strafe();
                }
            }
            else {
                if (isOvni) {
                    ovniAnimator.SetTrigger("strafeLeftTrigger");
                }
                else if (isTmaxFlying) {
                    tmaxAnimator.SetTrigger("strafeLeftTrigger");
                }
            }

            SoundManager.current.PlaySound(SoundType.STRAFE);
        }
        else {
            if (!(isTwingo || isTmax || isOvni)) {
                if (isCopFollowed) {
                    gameManager.Lose();
                    vfxManager.SetActiveVfxLoseOnFoot(true);
                    julAnim.Death();
                    cop.EndRun();
                    SoundManager.current.PlaySound(SoundType.FATAL_COLLISION);
                }
                else {
                    startCopFollowTimer = true;
                    julAnim.Hit();
                    vfxManager.PlayVfxHit();
                }

                cop.CatchUpToPlayer();
            }
        }
    }

    public void MoveToRight() {
        if (lane == Lane.LEFT) {
            lane = Lane.CENTER;
            isStrafing = true;
            playerTransform.DOMoveX(centerPos.position.x, moveTime).SetEase(Ease.Linear).OnComplete(() => StrafeComplete());
            copCatchupPos.DOMoveX(centerPos.position.x, moveTime).SetEase(Ease.Linear);
            cameraMovement.MoveToCenterPos(moveTime);
            parallaxe.MoveRight(moveTime);

            if (isGrounded) {
                if (isTwingo) {
                    twingoAnimator.SetTrigger("strafeRightTrigger");
                }
                else if (isTmax) {
                    tmaxAnimator.SetTrigger("strafeRightTrigger");
                }
                else {
                    julAnim.Strafe();
                    cop.Strafe();
                }
            }
            else {
                if (isOvni) {
                    ovniAnimator.SetTrigger("strafeRightTrigger");
                }
                else if (isTmaxFlying) {
                    tmaxAnimator.SetTrigger("strafeRightTrigger");
                }
            }

            SoundManager.current.PlaySound(SoundType.STRAFE);
        }
        else if (lane == Lane.CENTER) {
            lane = Lane.RIGHT;
            isStrafing = true;
            playerTransform.DOMoveX(rightPos.position.x, moveTime).SetEase(Ease.Linear).OnComplete(() => StrafeComplete());
            copCatchupPos.DOMoveX(rightPos.position.x, moveTime).SetEase(Ease.Linear);
            cameraMovement.MoveToRightPos(moveTime);
            parallaxe.MoveRight(moveTime);

            if (isGrounded) {
                if (isTwingo) {
                    twingoAnimator.SetTrigger("strafeRightTrigger");
                }
                else if (isTmax) {
                    tmaxAnimator.SetTrigger("strafeRightTrigger");
                }
                else {
                    julAnim.Strafe();
                    cop.Strafe();
                }
            }
            else {
                if (isOvni) {
                    ovniAnimator.SetTrigger("strafeRightTrigger");
                }
                else if (isTmaxFlying) {
                    tmaxAnimator.SetTrigger("strafeRightTrigger");
                }
            }

            SoundManager.current.PlaySound(SoundType.STRAFE);
        }
        else {
            if (!(isTwingo || isTmax || isOvni)) {
                if (isCopFollowed) {
                    gameManager.Lose();
                    vfxManager.SetActiveVfxLoseOnFoot(true);
                    julAnim.Death();
                    cop.EndRun();
                    SoundManager.current.PlaySound(SoundType.FATAL_COLLISION);
                }
                else {
                    startCopFollowTimer = true;
                    julAnim.Hit();
                    vfxManager.PlayVfxHit();
                }

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
            playerTransform.DOMoveY(topPos.position.y, cameraMovement.flyMoveTime).OnComplete(() => julCollider.enabled = true);
            cameraMovement.MoveToTopPos(); //moveTime * 2f

            isTmaxFlying = true;
            rb.useGravity = false;
            julCollider.enabled = false;
            isGrounded = false;

            itemManager.StartSpawnOvniDiscs();
        }

        startTmaxTimer = true;

        float spawnTime = Random.Range(0, tmaxDuration - 4f);
        Invoke("SpawnFlyingAlien", spawnTime);
    }

    public void EndFly() {
        if (isTmaxFlying) {
            tmaxTimer = 0f;
            GameEvents.current.AlienFail();
            isTmaxFlying = false;
            rb.useGravity = true;
            ActivateLook(jul);
            julAnim.SetFallBool(true);
            julAnim.PlayState("Fall");

            cameraMovement.MoveToGroundPos();
            cameraMovement.MoveToNormalPosZ();
        }

        if (isOvni) {
            ovniTimer = 0f;
            isOvni = false;
            rb.useGravity = true;
            ActivateLook(jul);
            julAnim.SetFallBool(true);
            julAnim.PlayState("Fall");

            cameraMovement.MoveToGroundPos();
            cameraMovement.MoveToNormalPosZ();
        }
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
                    SoundManager.current.PauseSound(SoundType.CLAQUETTES);
                }
                else {
                    rb.velocity = Vector3.up * jumpVelocity;
                    SoundManager.current.PauseSound(SoundType.RUN);
                }

                SoundManager.current.PlaySound(SoundType.JUMP);
            }
        }
    }

    public void HitByObstacle(Collider col) {
        Obstacles obstacle = col.gameObject.GetComponent<Obstacles>();

        if (col.tag == "Camionette") {
            if (isTwingo) {
                gameManager.Lose(DeathState.TWINGO);
                switch (obstacle.currentLane) {
                    case Lane.LEFT:
                        twingoAnimator.SetTrigger("deathTriggerRight");
                        break;
                    case Lane.CENTER:
                        int rand = Random.Range(0, 2);
                        if (rand == 0) twingoAnimator.SetTrigger("deathTriggerRight");
                        else twingoAnimator.SetTrigger("deathTriggerLeft");
                        break;
                    case Lane.RIGHT:
                        twingoAnimator.SetTrigger("deathTriggerLeft");
                        break;
                    default:
                        twingoAnimator.SetTrigger("deathTriggerLeft");
                        break;
                }
                vfxManager.PlayVfxExplosion();
                SoundManager.current.PlaySound(SoundType.FATAL_COLLISION);
            }
            else if (isTmax) {
                if (isY) gameManager.Lose(DeathState.TMAX, true);
                else gameManager.Lose(DeathState.TMAX, false);
                tmaxLose.StartLoseAnimation(obstacle.currentLane);
                yTimer = 0f;
                vfxManager.PlayVfxExplosion();
                SoundManager.current.PlaySound(SoundType.FATAL_COLLISION);
            }
            else {
                if (isStrafing && lane != obstacle.currentLane) {
                    if (isCopFollowed) {
                        gameManager.Lose();
                        vfxManager.SetActiveVfxLoseOnFoot(true);
                        julAnim.Death();
                        cop.EndRun();
                        SoundManager.current.PlaySound(SoundType.FATAL_COLLISION);
                    }
                    else {
                        startCopFollowTimer = true;
                        julAnim.Hit();
                        vfxManager.PlayVfxHit();
                        SoundManager.current.PlaySound(SoundType.COLLISION);
                    }
                }
                else {
                    gameManager.Lose();
                    vfxManager.SetActiveVfxLoseOnFoot(true);
                    julAnim.Death();
                    cop.EndRun();
                    SoundManager.current.PlaySound(SoundType.FATAL_COLLISION);
                }
                cop.CatchUpToPlayer();
            }
        }

        if (isTwingo) {
            if (col.tag == "Barriere" || col.tag == "Plot" || col.tag == "Rat" || col.tag == "Ballon") {
                gameManager.AddBreakItemScore();
                vfxManager.PlayVfxBroken();
                obstacle.Throw();
            }
            else if (col.tag == "Voiture") {
                gameManager.Lose(DeathState.TWINGO);
                switch (obstacle.currentLane) {
                    case Lane.LEFT:
                        twingoAnimator.SetTrigger("deathTriggerRight");
                        break;
                    case Lane.CENTER:
                        int rand = Random.Range(0, 2);
                        if (rand == 0) twingoAnimator.SetTrigger("deathTriggerRight");
                        else twingoAnimator.SetTrigger("deathTriggerLeft");
                        break;
                    case Lane.RIGHT:
                        twingoAnimator.SetTrigger("deathTriggerLeft");
                        break;
                    default:
                        twingoAnimator.SetTrigger("deathTriggerLeft");
                        break;
                }
                vfxManager.PlayVfxExplosion();
                SoundManager.current.PlaySound(SoundType.FATAL_COLLISION);
            }
        }
        else if (isTmax) {
            if (isY) {
                if (col.tag == "Voiture") {
                    gameManager.AddBreakItemScore(true);
                    obstacle.Throw();
                    vfxManager.PlayVfxBroken();
                }
            }
            else {
                if (col.tag == "Voiture") {
                    gameManager.Lose(DeathState.TMAX);
                    tmaxLose.StartLoseAnimation(obstacle.currentLane);
                    vfxManager.PlayVfxExplosion();
                    SoundManager.current.PlaySound(SoundType.FATAL_COLLISION);
                }
            }

            if (col.tag == "Barriere" || col.tag == "Plot" || col.tag == "Rat" || col.tag == "Ballon") {
                gameManager.AddBreakItemScore();
                obstacle.Throw();
                vfxManager.PlayVfxBroken();
            }
        }
        else {
            if (col.tag == "Barriere" || col.tag == "Plot" || col.tag == "Rat" || col.tag == "Ballon") {   //obstacles légers a pied
                if (isCopFollowed) {
                    gameManager.Lose();
                    vfxManager.SetActiveVfxLoseOnFoot(true);
                    julAnim.Death();
                    cop.EndRun();
                    SoundManager.current.PlaySound(SoundType.FATAL_COLLISION);
                }
                else {
                    startCopFollowTimer = true;
                    julAnim.Hit();
                    obstacle.Throw();
                    vfxManager.PlayVfxHit();
                    SoundManager.current.PlaySound(SoundType.COLLISION);
                }
                cop.CatchUpToPlayer();
            }
            else if (col.tag == "Voiture") {
                if (isStrafing && lane != obstacle.currentLane) {
                    if (isCopFollowed) {
                        gameManager.Lose();
                        vfxManager.SetActiveVfxLoseOnFoot(true);
                        julAnim.Death();
                        cop.EndRun();
                        SoundManager.current.PlaySound(SoundType.FATAL_COLLISION);
                    }
                    else {
                        startCopFollowTimer = true;
                        julAnim.Hit();
                        vfxManager.PlayVfxHit();
                        SoundManager.current.PlaySound(SoundType.COLLISION);
                    }
                }
                else {
                    gameManager.Lose();
                    vfxManager.SetActiveVfxLoseOnFoot(true);
                    julAnim.Death();
                    cop.EndRun();
                    SoundManager.current.PlaySound(SoundType.FATAL_COLLISION);
                }
                cop.CatchUpToPlayer();
            }
        }
    }

    public void HitByBonus(string tag) {
        SoundManager.current.PlaySound(SoundType.COINS);
        switch (tag) {
            case "Claquettes":
                startClaquettesTimer = true;
                SoundManager.current.PlaySound(SoundType.CLAQUETTES);
                SoundManager.current.PauseSound(SoundType.RUN);
                break;
            case "Pochon":
                startPochonTimer = true;
                break;
            case "Twingo":
                if (isClaquettes) claquettesTimer = 0f;

                copFollowTimer = 0f;
                startTwingoTimer = true;
                ActivateLook(twingo);
                twingoOvniCollider.enabled = true;
                julCollider.enabled = false;
                vfxManager.PlayVfxBonus();
                SoundManager.current.PlaySound(SoundType.TWINGO);

                if (!isTwingo)
                    Time.timeScale += twingoSpeed;
                break;
            case "Tmax":
                if (isClaquettes) claquettesTimer = 0f;

                copFollowTimer = 0f;
                startTmaxTimer = true;
                ActivateLook(tmax);
                vfxManager.PlayVfxBonus();
                SoundManager.current.PlaySound(SoundType.TMAX);

                if (!isTmax) {
                    Time.timeScale += tmaxSpeed;
                    AlienEvent();
                }
                break;
            default:
                break;
        }
    }

    public void StartOvni() {
        copFollowTimer = 0f;
        startOvniTimer = true;
        ActivateLook(ovni);
        twingoOvniCollider.enabled = true;
        julCollider.enabled = false;
        gameManager.IncrementAlienMoney();

        SoundManager.current.PlaySound(SoundType.OVNI);
        SoundManager.current.StopSound(SoundType.TMAX);
    }

    public void EndOvni() {
        ovniTimer = 0;
        SoundManager.current.StopSound(SoundType.OVNI);
    }

    public void HitByDisc(bool isGold) {
        SoundManager.current.PlaySound(SoundType.COINS);
        vfxManager.PlayVfxDisc();
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
        if (!go.activeInHierarchy) {
            jul.SetActive(false);
            twingo.SetActive(false);
            tmax.SetActive(false);
            ovni.SetActive(false);

            go.SetActive(true);
        }
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
            vfxManager.SetActiveVfxShoes(true);

            startClaquettesTimer = false;
        }

        if (isClaquettes) {
            if (claquettesTimer <= 0f) {
                isClaquettes = false;
                vfxManager.SetActiveVfxShoes(false);
                SoundManager.current.StopSound(SoundType.CLAQUETTES);
                SoundManager.current.UnPauseSound(SoundType.RUN);
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
            vfxManager.SetActiveVfxWeed(true);

            startPochonTimer = false;
        }

        if (isPochon) {
            if (pochonTimer <= 0f) {
                isPochon = false;
                vfxManager.SetActiveVfxWeed(false);
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
                twingoOvniCollider.enabled = false;
                julCollider.enabled = true;
                SoundManager.current.StopSound(SoundType.TWINGO);

                Time.timeScale -= twingoSpeed;
            }
            else {
                if (!(gameManager.gameState == GameState.PAUSE || gameManager.gameState == GameState.FINISHED)) twingoTimer -= Time.unscaledDeltaTime;
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
                    SoundManager.current.StopSound(SoundType.TMAX);

                    Time.timeScale -= tmaxSpeed;
                }
            }
            else {
                if (!(gameManager.gameState == GameState.PAUSE || gameManager.gameState == GameState.FINISHED)) tmaxTimer -= Time.unscaledDeltaTime;
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
            ovniTimer = 200f;

            Time.timeScale -= tmaxSpeed;
            Time.timeScale += ovniSpeed;

            cameraMovement.MoveToOvniPosY();   //moveTime * 2f
            cameraMovement.MoveToOvniPosZ();   //moveTime * 2f

            startOvniTimer = false;
        }

        if (isOvni) {
            if (ovniTimer <= 0f) {
                twingoOvniCollider.enabled = false;
                julCollider.enabled = true;

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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(centerPos.position, obstacleDetectionRadius);
    }
    #endregion
}
