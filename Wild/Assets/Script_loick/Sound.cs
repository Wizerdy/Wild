using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Sound : MonoBehaviour
{
    public SoundExecuter SE;
    private void Awake()
    {
     gameObject.AddComponent<SpriteRenderer>();
     var sprite = GetComponent<SpriteRenderer>();
     sprite.sprite = Resources.Load<Sprite>("SpriteTest/Circle");
    }
    private void Update()
    {
        if (SE.soundname != Sound_Manager.soundname.FT) 
        {
            Destroy(GetComponent<SpriteRenderer>());
        }
       
    }
}
