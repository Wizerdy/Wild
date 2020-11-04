using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surbrillance : MonoBehaviour
{
    public Material surbrillance;
    public Material BaseM;
    private MeshRenderer Mesh;
    private bool ShinyOrNot = false;
    
    
    void Start()
    {
        Mesh = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
       
        
    }

    public void Shine() 
    {
        ShinyOrNot = !ShinyOrNot;

        if (ShinyOrNot)
        {
            Mesh.sharedMaterial = surbrillance;
        }
        if (!ShinyOrNot)
        {
            Mesh.sharedMaterial = BaseM;
        }
    
    
    }
}
