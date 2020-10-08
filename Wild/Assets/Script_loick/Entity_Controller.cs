using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_Controller : MonoBehaviour
{
    public Player entity;

    void Update()
    {
        
        Vector2 dir_Move = Vector2.zero;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            dir_Move.x = -1f;
        }
        else if(Input.GetKey(KeyCode.RightArrow))
        {
            dir_Move.x = 1f;
        }


        if (Input.GetKey(KeyCode.UpArrow))
        {
            dir_Move.y = 1f;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            dir_Move.y = -1f;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            entity.Dash();
        }

          entity.Move(dir_Move);
    }
}
