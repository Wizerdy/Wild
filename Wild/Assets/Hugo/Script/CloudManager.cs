using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour
{
    public GameObject cloudPrefab;
    public Transform depart; 
    public Transform arrivé;
    private Transform cloud;
    public float speed;
    private float actualPos = 0;
    // Start is called before the first frame update
    void Start()
    {
        spawn();
    }

    // Update is called once per frame
    void Update()
    {
        
        actualPos += speed*Time.deltaTime ;
        cloud.position = Vector3.Lerp(depart.position, arrivé.position, actualPos);
        if (actualPos > 1)
        {
            actualPos = 0;
            cloud.position = depart.position;
        }
    }

    public void spawn() 
    {
        
      cloud =  Instantiate(cloudPrefab, arrivé.position,Quaternion.identity).transform;
        
    }
}
