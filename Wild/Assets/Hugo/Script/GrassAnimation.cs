using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassAnimation : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Lion")
        {
            anim.SetBool("Contact", true);
        }
        
        
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Lion")
        {
            anim.SetBool("Contact", false);
        }
    }

}
