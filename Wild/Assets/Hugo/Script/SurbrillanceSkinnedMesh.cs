using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurbrillanceSkinnedMesh : MonoBehaviour
{
    public Material surbrillance;
    private Material baseM;
    private SkinnedMeshRenderer Sp;



    void Start()
    {
        Sp = GetComponent<SkinnedMeshRenderer>();
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
