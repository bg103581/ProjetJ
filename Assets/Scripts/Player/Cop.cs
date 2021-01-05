using DG.Tweening;
using UnityEngine;

public class Cop : MonoBehaviour
{
    
    private bool isFollowingPlayer;
    private CopStartAnimMovement copStartAnimMovement;
    private GameManager gameManager;

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

        GameEvents.current.onReplayButtonTrigger += OnReplay;
        GameEvents.current.onMainMenuButtonTrigger += OnReplay;
    }

    private void Update() {
        if (isFollowingPlayer && gameManager.gameState != GameState.PAUSE) {
            transform.DOMove(copCatchUpPos.position, catchUpMoveTime);
        }
    }

    private void OnDestroy() {
        GameEvents.current.onReplayButtonTrigger -= OnReplay;
        GameEvents.current.onMainMenuButtonTrigger -= OnReplay;
    }

    private void OnReplay() {
        transform.position = initialPos.position;
        isFollowingPlayer = false;
        //settrigger anim to go back to init state
        anim.Play("Idle");
    }

    public void CatchUpToPlayer() {
        isFollowingPlayer = true;
    }

    public void GoBackToInitialPos() {
        isFollowingPlayer = false;
        transform.DOMove(initialPos.position, backToInitMoveTime);
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
}
