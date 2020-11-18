using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surbrillance3d : MonoBehaviour
{
    public Material surbrillance;
    private Material baseM;
    private MeshRenderer Mesh;
    


    void Start()
    {
        Mesh = GetComponent<MeshRenderer>();
        baseM = GetComponent<MeshRenderer>().material; 
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
