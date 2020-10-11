using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_Controller : MonoBehaviour
{
    public Entity entity;

    void Update()
    {
        Vector2 dir_Move = Vector2.zero;

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.Q)) {
            dir_Move += Vector2.left;
        }
        else if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
            dir_Move += Vector2.right;
        }

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Z)) {
            dir_Move += Vector2.up;
        }
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
            dir_Move += Vector2.down;
        }

        if (Input.GetKeyDown(KeyCode.Space) && entity.CanDash) {
            entity.Dash();
        }

        entity.MoveDir(dir_Move);
    }

    protected void OnCollisionEnter(Collision collide) {
        if (collide.gameObject.tag == "Enemy") {
            Lose();
        }
    }

    protected void OnTriggerEnter(Collider collide) {
        if(collide.gameObject.tag == "End") {
            Debug.Log("Fin");
            NextLevel();
        }
    }

    protected void Lose() {
        Tools.LoadScene(0);
    }

    protected void NextLevel() {
        Application.Quit();
    }
}
