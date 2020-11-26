using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //speed
    [Header("Speed")]
    public float speedMax;
    public float acceleration;

    //force 
    [Header("Force")]
    private Vector2 velocity= Vector2.zero;
    private Rigidbody2D rb = null;
    private Vector2 move_dir = Vector2.zero;

    //friction
    [Header("Friction")]
    public float friction;
    public float trunFriction = 50f;

    //dash
    [Header("Dash")]
    public float dashSpeed;
    public float dashDuration;
    public float dashCountdown = -1f;
    public bool cooldown;
    private Vector2 dash_dir = Vector2.zero;

    //Soundmanage
    private Sound_Manager s_Manager;
    private float cooldownFootStep = 0.5f;
    private float nextstep;

    public void Dash()
    {
        cooldown = true;
        dashCountdown = dashDuration;
        dash_dir = transform.eulerAngles;
    }
    private void _UpdateDash()
    {
        dashCountdown -= Time.fixedDeltaTime;
        if (dashCountdown<0f)
        {
            velocity = dash_dir * dashSpeed;
        }
        else
        {
            velocity = dash_dir * speedMax;
            cooldown = false;
        }
    }

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        s_Manager = GameObject.Find("SoundManager").GetComponent<Sound_Manager>();
    }
    public void Move(Vector2 dir)
    {
        move_dir = dir.normalized;
    }

      void FixedUpdate()
    {
        if (cooldown)
        {
         _UpdateDash();
        }
        _UpdateMove();
        _ApplySpeed();
    }

    private void Update()
    {
        if (rb.velocity != Vector2.zero && gameObject.tag == "Player")
        {
            s_Manager.soundclass = Sound_Manager.soundname.FT;
            if (Time.time> nextstep)
            {
            s_Manager.GenerateSound(s_Manager.soundclass,gameObject.transform.localPosition);
                nextstep = Time.time + cooldownFootStep;
            }

        }
        else
        {
            s_Manager.soundclass = Sound_Manager.soundname.AMB;
        }
    }
    private void _ApplySpeed()
    {
        if (null !=rb)
        {
            rb.velocity = velocity;
        }
        else
        {
        Vector2 pos = transform.position;
        pos+= velocity * Time.fixedDeltaTime;
        transform.position = pos;
        }

    }
    private void _UpdateMove()
    {
        if (move_dir != Vector2.zero)
        {
            float angle = Vector2.SignedAngle(velocity, move_dir);
            float angleRatio = Mathf.Abs(angle) / 360f;
            float frictionToApply = trunFriction * angleRatio * Time.fixedDeltaTime;
            Vector2 frictionDir = velocity.normalized;
            velocity -= frictionDir * frictionToApply;

            velocity += move_dir * acceleration * Time.fixedDeltaTime;
            if (velocity.sqrMagnitude > speedMax * speedMax)
            {
                velocity = velocity.normalized * speedMax;
            }

           
        }
        else
        {
            float frictiontoapply = friction * Time.fixedDeltaTime;
            if (velocity.sqrMagnitude >  frictiontoapply * frictiontoapply)
            {
                velocity -= velocity.normalized * frictiontoapply;
            }
            else
            {
                velocity = Vector2.zero;
            }
        }
    }
}
