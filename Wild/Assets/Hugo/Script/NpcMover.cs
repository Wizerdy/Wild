using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcMover : MonoBehaviour
{

    public GameObject Target;
    public GameObject InitTarget;
    
    public bool Routine;
    private NavMeshAgent NavA;

    // Start is called before the first frame update
    void Start()
    {
        NavA = gameObject.GetComponent<NavMeshAgent>();
        Routine = true;
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Routine)
        {
            NavA.destination = Target.transform.position;
        }
        else
        {
            NavA.destination = InitTarget.transform.position;
        }

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            Routine = false;
        }
        if (other.tag == "Grass")
        {
            Routine = true;
        }

    }
}

    
