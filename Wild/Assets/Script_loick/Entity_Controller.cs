using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Entity_Controller : MonoBehaviour {
    public Entity player;
    public string _rewiredPlayerName = "Player0";
    private Rewired.Player _rewiredPlayer = null;
    private Vector2 dirMove;
    private float runSpeed;
    private float walkSpeed;
    private Animator animator;
    private bool isWalking;
    private Vector2 lateDirMove;
    public SoundManager.SoundObject DashSound;

    private HideZone hideZone = null;

    private void Awake() {
        animator = GetComponentInChildren<Animator>();
    }

    private void Start() {
        runSpeed = player.speedMaxGlobal;
        walkSpeed = player.speedMaxGlobal / 2;
        _rewiredPlayer = ReInput.players.GetPlayer(_rewiredPlayerName);
    }

    void Update() {
        dirMove.x = _rewiredPlayer.GetAxis("Horizontal");
        dirMove.y = _rewiredPlayer.GetAxis("Vertical");

        #region Snake
        if (player.underEffect) {
            runSpeed = player.speedMaxGlobal;
            walkSpeed = player.speedMaxGlobal / 2;
            Tools.ChangePostProcessingProfile(GameManager.Instance.snakeProfile);
        } else {
            runSpeed = player.defaultSpeedMax;
            walkSpeed = player.defaultSpeedMax / 2;
            Tools.ChangePostProcessingProfile(GameManager.Instance.defaultProfile);
        }
        #endregion

        #region Speed
        if (dirMove.sqrMagnitude < 0.7f * 0.7f) {
            player.speedMax = walkSpeed;
            isWalking = true;
        } else {
            player.speedMax = runSpeed;
            isWalking = false;
        }
        #endregion

        #region Dash
        if (_rewiredPlayer.GetButtonDown("Dash")) {
            DashSound.Play();
            player.Dash();
        }
        #endregion

        #region Instinct
        if (_rewiredPlayer.GetButtonDown("Instinct")) {
            FindObjectOfType<SurbrillanceTrigger>().ActiveInstinctMode();
            //player.GetComponentInChildren<SurbrillanceTrigger>().ActiveInstinctMode();
        }
        #endregion

        #region Hide
        if (_rewiredPlayer.GetButtonDown("Hide") && hideZone != null) {
            player.GetComponent<LionCubEntity>().Hide(hideZone);
            Debug.LogWarning(player.GetComponent<LionCubEntity>().hidden);
        }
        #endregion

        #region Animator
        if (animator != null && isWalking) {
            animator.SetBool("Running", true);
            animator.SetBool("Walking", false);
            animator.SetFloat("MoveX", -dirMove.x);
            animator.SetFloat("MoveY", dirMove.y);
            if (dirMove != Vector2.zero) {
                lateDirMove.x = -dirMove.x;
                lateDirMove.y = dirMove.y;
            }
            //animator.SetBool("Moving", dirMove != Vector2.zero);
        } else {
            animator.SetBool("Walking", true);
            animator.SetBool("Running", false);
            animator.SetFloat("MoveX", -dirMove.x);
            animator.SetFloat("MoveY", dirMove.y);
            if (dirMove != Vector2.zero) {
                lateDirMove.x = -dirMove.x;
                lateDirMove.y = dirMove.y;
            }
        }

        if (null != animator && dirMove == Vector2.zero) {
            animator.SetBool("Walking", false);
            animator.SetBool("Running", false);
            animator.SetFloat("MoveX", lateDirMove.x);
            animator.SetFloat("MoveY", lateDirMove.y);
        }
        #endregion

        player.MoveDir(dirMove);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<HideZone>() != null) {
            hideZone = other.GetComponent<HideZone>();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.GetComponent<HideZone>() != null) {
            hideZone = null;
        }
    }

    //private void OnTriggerStay(Collider other) {
    //    if (_rewiredPlayer.GetButtonDown("Hide") && !(null == other.GetComponent<HideZone>())) {
    //        player.GetComponent<LionCubEntity>().Hide(other.GetComponent<HideZone>());
    //        Debug.LogWarning(player.GetComponent<LionCubEntity>().hidden);
    //    }
    //}
}
