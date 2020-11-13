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
    private float crouchSpeed;
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Start() {
        runSpeed = player.speedMax;
        walkSpeed = player.speedMax / 2;
        crouchSpeed = player.speedMax / 3;
        _rewiredPlayer = ReInput.players.GetPlayer(_rewiredPlayerName);
    }

    void Update() {
        dirMove.x = _rewiredPlayer.GetAxis("Horizontal");
        dirMove.y = _rewiredPlayer.GetAxis("Vertical");

        if (dirMove.x == Mathf.Clamp(dirMove.x, -0.3f, 0.3f) && dirMove.y == Mathf.Clamp(dirMove.y, -0.3f, 0.3f)) {
            player.speedMax = crouchSpeed;
        }
        else if (dirMove.x == Mathf.Clamp(dirMove.x, -0.6f, 0.6f) && dirMove.y == Mathf.Clamp(dirMove.y, -0.6f, 0.6f)) {
            player.speedMax = walkSpeed;
        }
        else {
            player.speedMax = runSpeed;
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            player.Dash();
        }

        if (animator != null) { animator.SetFloat("MoveX", -dirMove.x); animator.SetFloat("MoveY", dirMove.y); animator.SetBool("Moving", dirMove != Vector2.zero); }

        player.MoveDir(dirMove);
    }
}
