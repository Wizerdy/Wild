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

    private void Awake() {
        animator = GetComponentInChildren<Animator>();
    }

    private void Start() {
        runSpeed = player.speedMax;
        walkSpeed = player.speedMax / 2;
        _rewiredPlayer = ReInput.players.GetPlayer(_rewiredPlayerName);
    }


    void Update() {
        dirMove.x = _rewiredPlayer.GetAxis("Horizontal");
        dirMove.y = _rewiredPlayer.GetAxis("Vertical");

        if (dirMove.x == Mathf.Clamp(dirMove.x, -0.9f, 0.9f) && dirMove.y == Mathf.Clamp(dirMove.y, -0.9f, 0.9f)) {
            player.speedMax = walkSpeed;
            isWalking = true;
        }
        else {
            player.speedMax = runSpeed;
            isWalking = false;
        }

        if (_rewiredPlayer.GetButtonDown("Dash")) {
            player.Dash();
        }

        if (animator != null && isWalking)
        {
            animator.SetBool("Running", true);
            animator.SetBool("Walking", false);
            animator.SetFloat("MoveX", -dirMove.x);
            animator.SetFloat("MoveY", dirMove.y);
            if (dirMove != Vector2.zero)
            {
                lateDirMove.x = -dirMove.x;
                lateDirMove.y = dirMove.y;
            }
            //animator.SetBool("Moving", dirMove != Vector2.zero);
        } else
        {
            animator.SetBool("Walking", true);
            animator.SetBool("Running", false);
            animator.SetFloat("MoveX", -dirMove.x);
            animator.SetFloat("MoveY", dirMove.y);
            if (dirMove != Vector2.zero)
            {
                lateDirMove.x = -dirMove.x;
                lateDirMove.y = dirMove.y;
            }
        } 

        if (null != animator && dirMove == Vector2.zero)
        {
            animator.SetBool("Walking", false);
            animator.SetBool("Running", false);
            animator.SetFloat("MoveX", lateDirMove.x);
            animator.SetFloat("MoveY", lateDirMove.y);
        }

        player.MoveDir(dirMove);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!(null == other.GetComponent<HideZone>()) && _rewiredPlayer.GetButtonDown("Hide"))
        {
            player.GetComponent<LionCubEntity>().Hide(other.GetComponent<HideZone>());
            Debug.Log(player.GetComponent<LionCubEntity>().hidden);
        }
    }
}
