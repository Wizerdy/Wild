using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideIn : MonoBehaviour
{
    private bool canHide;

    private void Update()
    {
        if (canHide && Input.GetKeyDown(KeyCode.Space))
        {
            //Hide();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "HideZone")
        {
            canHide = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "HideZone")
        {
            canHide = false;
        }
    }

   
}
