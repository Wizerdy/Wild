using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surbrillance2d : MonoBehaviour
{
    public Material surbrillance;
    private Material baseM;
    private SpriteRenderer Sp;



    void Start()
    {
        Sp = GetComponent<SpriteRenderer>();
        baseM = GetComponent<SpriteRenderer>().material;
    }

    

    public void Shine(bool ShinyOrNot)
    {

        if (ShinyOrNot)
        {
            Sp.material = surbrillance;
        }
        if (!ShinyOrNot)
        {
            Sp.material = baseM;
        }


    }
}
