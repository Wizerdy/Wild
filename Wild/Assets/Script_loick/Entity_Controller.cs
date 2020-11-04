using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Entity_Controller : MonoBehaviour {
    public Entity player;
    private string _rewiredPlayerName = "Player0";
    private Rewired.Player _rewiredPlayer = null;
    public float joyaxeX;
    public float joyaxeY;
    public bool test;
    public float run;
    public float walk;

    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Start() {
        run = player.speedMax;
        walk = player.speedMax / 2;
        _rewiredPlayer = ReInput.players.GetPlayer(_rewiredPlayerName);
    }

    void Update() {
        joyaxeX = _rewiredPlayer.GetAxis("Horizontal");
        joyaxeY = _rewiredPlayer.GetAxis("Vertical");

        Vector2 dir_Move = Vector2.zero;
        if (joyaxeX == Mathf.Clamp(joyaxeX, -0.9f, 0.9f) && joyaxeY == Mathf.Clamp(joyaxeY, -0.9f, 0.9f)) {
            test = true;
            dir_Move.x = joyaxeX;
            dir_Move.y = joyaxeY;
            player.speedMax = walk;
        } else {
            test = false;
            dir_Move.x = joyaxeX;
            dir_Move.y = joyaxeY;
            player.speedMax = run;
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            player.Dash();
        }

        if (animator != null) { animator.SetFloat("MoveX", -dir_Move.x); animator.SetFloat("MoveY", dir_Move.y); animator.SetBool("Moving", dir_Move != Vector2.zero); }

        player.MoveDir(dir_Move);
    }
}
