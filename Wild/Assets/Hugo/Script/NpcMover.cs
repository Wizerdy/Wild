using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcMover : MonoBehaviour
{
    public bool sneak;
    public GameObject target;
    public GameObject initTarget;
    
    public bool routine;
    private NavMeshAgent navA;

    // Start is called before the first frame update
    void Start()
    {
        navA = gameObject.GetComponent<NavMeshAgent>();
        routine = true;
        
    }

    // Update is called once per frame
    void Update()
    {

        if (routine)
        {
            navA.destination = target.transform.position;
        }
        else
        {
            navA.destination = initTarget.transform.position;
        }

    }

    

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            routine = false;
        }
        if (other.tag == "Grass")
        {
            routine = true;
        }
        if (other.tag == "Hide")
        {
            sneak = true;
 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Hide")
        {
            sneak = false;

        }
    }
}

    
