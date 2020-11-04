using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurbrillanceTrigger : MonoBehaviour
{
    public GameObject triggerzone;
       
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            trigger();
        } 
    }

    public void trigger() 
    {
        triggerzone.transform.localScale = new Vector3(3,1,1);
    }
}
