using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_Controller : MonoBehaviour
{
    public Player player;
    public float joyaxeX;
    public float joyaxeY;
    public bool test;
    public float run;
    public float walk;
    private void Start()
    {
        run = player.speedMax;
        walk = player.speedMax / 2;
    }
    void Update()
    {
        joyaxeX = Input.GetAxis("Horizontal");
        joyaxeY = Input.GetAxis("Vertical");

        Vector2 dir_Move = Vector2.zero;
        if (joyaxeX >0 && joyaxeX != 1 || joyaxeX<0 && joyaxeX != -1|| joyaxeY > 0 && joyaxeY != 1 || joyaxeY < 0 && joyaxeY != -1)
        {
            test = true;
            dir_Move.x =joyaxeX;
            dir_Move.y =joyaxeY;
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

          player.Move(dir_Move);
    }
}
