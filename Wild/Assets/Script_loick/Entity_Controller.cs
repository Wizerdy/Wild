using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Entity_Controller : MonoBehaviour
{
    public Entity player;
    private string _rewiredPlayerName = "Player0";
    private Rewired.Player _rewiredPlayer = null;
    public float joyaxeX;
    public float joyaxeY;
    public bool test;
    public float run;
    public float walk;

    private void Start()
    {
        run = player.speedMax;
        walk = player.speedMax / 2;
        _rewiredPlayer = ReInput.players.GetPlayer(_rewiredPlayerName);
    }

    void Update()
    {
        joyaxeX = _rewiredPlayer.GetAxis("MoveHorizontal");
        joyaxeY = _rewiredPlayer.GetAxis("MoveVertical");

        Vector2 dir_Move = Vector2.zero;
        if (joyaxeX == Mathf.Clamp(joyaxeX, -0.9f, 0.9f) && joyaxeY == Mathf.Clamp(joyaxeY, -0.9f, 0.9f))
        {
            test = true;
            dir_Move.x = joyaxeX;
            dir_Move.y = joyaxeY;
            player.speedMax = walk;
        }
        else
        {
            test = false;
            dir_Move.x = joyaxeX;
            dir_Move.y = joyaxeY;
            player.speedMax = run;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.Dash();
        }

        player.MoveDir(dir_Move);
    }
}
