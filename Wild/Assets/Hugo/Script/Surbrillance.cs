using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surbrillance : MonoBehaviour
{
    public Material surbrillance;
    private Material baseM;
    private SkinnedMeshRenderer Mesh;
    


    void Start()
    {
        Mesh = GetComponent<SkinnedMeshRenderer>();
        baseM = GetComponent<SkinnedMeshRenderer>().material; 
    }

    // Update is called once per frame
    void Update()
    {
       
        
    }

    public void Shine(bool ShinyOrNot) 
    {
        
        if (ShinyOrNot)
        {
            Mesh.sharedMaterial = surbrillance;
        }
        if (!ShinyOrNot)
        {
            Mesh.sharedMaterial = baseM;
        }
    
    
    }
}
