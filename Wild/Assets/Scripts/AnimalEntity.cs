using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalEntity : Entity
{
    public bool hidden;
    protected int hideCoat;
    public string hideId = "";

    protected virtual void OnTriggerEnter(Collider collide)
    {
        if (collide.gameObject.tag == "Hide")
        {
            if (collide.gameObject.GetComponent<Entity>() != null)
            {
                hideId = collide.gameObject.GetComponent<Entity>().entityId;
            }
            hideCoat++;
            hidden = true;
        }
    }

    protected virtual void OnTriggerExit(Collider collide)
    {
        if (collide.gameObject.tag == "Hide")
        {
            hideCoat--;
            if (hideCoat <= 0)
            {
                hidden = false;
                hideId = "";
            }
        }
    }
}
