using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surbrillance : MonoBehaviour
{
    public Material surbrillance;
    private Material baseM;
    private SkinnedMeshRenderer Mesh;
    private Material baseM2;
    private MeshRenderer Mesh2;


    void Start()
    {
        if (!(null == GetComponent<SkinnedMeshRenderer>()))
        {
            Mesh = GetComponent<SkinnedMeshRenderer>();
            baseM = GetComponent<SkinnedMeshRenderer>().material;
        }
        if (!(null == GetComponent<MeshRenderer>()))
        {
            Mesh2 = GetComponent<MeshRenderer>();
            baseM2 = GetComponent<MeshRenderer>().material;
        }
    }

    // Update is called once per frame
    void Update()
    {
       
        
    }

    public void Shine(bool ShinyOrNot) 
    {
        if (!(null == Mesh))
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
        if (!(null == Mesh2))
        {
            if (ShinyOrNot)
            {
                Mesh2.sharedMaterial = surbrillance;
            }
            if (!ShinyOrNot)
            {
                Mesh2.sharedMaterial = baseM2;
            }
        }
    }
}
