using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Carnivore : MonoBehaviour
{
    public GameObject chased;
    private NavMeshAgent NavA;
    public List <GameObject> Targets = new List<GameObject>();
    public Transform target;
    public bool Chasing;
   
    public Vector3 InitPos;

    // Start is called before the first frame update
    void Start()
    {
        InitPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);

        NavA = gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBool();
       
        
        if (Chasing )
        {
            NavA.destination = chased.transform.position;
        }
        
        if (!Chasing )
        {
            
            NavA.destination = target.position;
        }

    }

    private void UpdateBool()
    {
        
        
        if (chased != null)
        {
            Chasing = true;
        }
        else
        {
            Chasing = false;
        }
    }
    private void UpdateTarget(int id) 
    {
        switch (id) 
        {
            case 0: target = Targets[1].transform;
                break;
            case 1: target = Targets[2].transform;
                break;
            case 2: target = Targets[0].transform;
                break;
        }
            

           
    }
    public void Detected(GameObject newTarget)
    {
        Debug.Log("detected");
        chased = newTarget;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Lair")
        {
            Chasing = false;
            chased = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Target")
        {
            UpdateTarget(other.gameObject.GetComponent<Target>().ID);
        }
    }
}

