using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surbrillance3d : MonoBehaviour
{
    public Material surbrillance;
  
    private Material baseM;
    private MeshRenderer Mesh;
    private List<Material> baseMats = new List<Material>();
    private List<MeshRenderer> MeshList = new List<MeshRenderer>();


    void Start()
    {
        
        if (transform.childCount == 0)
        {
            Mesh = GetComponent<MeshRenderer>();
            baseM = GetComponent<MeshRenderer>().material;

        }
        if (transform.childCount !=0)
        {

            for (int i = 0; i < transform.childCount; i++)
            {
                MeshList.Add(transform.GetChild(i).GetComponent<MeshRenderer>());
                baseMats.Add(MeshList[i].material);
            }
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
       
        
    }

    public void Shine(bool ShinyOrNot) 
    {
        
        if (transform.childCount == 0)
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

        if (transform.childCount != 0)
        {
            
            for (int i = 0; i < transform.childCount; i++)
            {
                
                if (ShinyOrNot)
                {
                    
                    MeshList[i].material = surbrillance;
                }
                if (!ShinyOrNot)
                {
                    
                    MeshList[i].material = baseMats[i];
                }
            }
            
        }
    
    
    }
}
