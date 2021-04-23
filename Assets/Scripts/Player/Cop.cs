using DG.Tweening;
using UnityEngine;

public class Cop : MonoBehaviour
{
    
    private bool isFollowingPlayer;
    private CopStartAnimMovement copStartAnimMovement;
    private GameManager gameManager;
    private Transform copTransform = null;

    private Vector3 initCopCatchUpPos;

    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Transform initialPos;
    public Transform copCatchUpPos;
    [SerializeField]
    private float catchUpMoveTime;
    [SerializeField]
    private float backToInitMoveTime;

    private void Awake() {
        copStartAnimMovement = GetComponent<CopStartAnimMovement>();
        gameManager = FindObjectOfType<GameManager>();
        copTransform = transform;

        GameEvents.current.onReplayButtonTrigger += OnReplay;
        GameEvents.current.onMainMenuButtonTrigger += OnReplay;
        GameEvents.current.onPreContinueGame += OnPreContinue;

        initCopCatchUpPos = copCatchUpPos.position;

        copTransform.SetParent(null);
        copCatchUpPos.SetParent(null);
        initialPos.SetParent(null);
    }

    private void Update() {
        if (isFollowingPlayer && gameManager.gameState != GameState.PAUSE) {
            copTransform.DOMove(copCatchUpPos.position, catchUpMoveTime);
        }
    }

    private void OnDestroy() {
        GameEvents.current.onReplayButtonTrigger -= OnReplay;
        GameEvents.current.onMainMenuButtonTrigger -= OnReplay;
        GameEvents.current.onPreContinueGame -= OnPreContinue;
    }

    private void OnReplay() {
        copCatchUpPos.position = initCopCatchUpPos;
        copTransform.position = initialPos.position;
        isFollowingPlayer = false;
        anim.Play("Idle");
    }

    private void OnPreContinue()
    {
        Debug.Log("cop pre continue");
        copTransform.DOKill();
        copTransform.position = initialPos.position;
        isFollowingPlayer = false;
        anim.Play("Run");
    }

    public void CatchUpToPlayer() {
        isFollowingPlayer = true;
    }

    public void GoBackToInitialPos() {
        if (gameManager.gameState != GameState.FINISHED) {
            isFollowingPlayer = false;
            copTransform.DOKill();
            copTransform.DOMove(initialPos.position, backToInitMoveTime);
        }
    }

    public void Jump() {
        anim.SetTrigger("jumpTrigger");
    }

    public void Strafe() {
        anim.SetTrigger("strafeTrigger");
    }

    public void StartAnimation() {
        anim.SetTrigger("startAnimationTrigger");

        copStartAnimMovement.PlayStartMovement();
    }

    public void TriggerSuprisedAnimation() {
        anim.SetTrigger("surprisedTrigger");
    }

    public void EndRun() {
        anim.SetTrigger("endRunTrigger");
    }
}
