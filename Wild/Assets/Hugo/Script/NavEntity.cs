using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavEntity : MonoBehaviour
{
    public Transform destination;
    public string iD;
    public float speed = 5f;

    private NavMeshPath navPath;
    public float refreshPathDuration = 0.1f;
    private float refreshPathCountdown = -1f;
    private int followPathIndex = 0;

    
    void Start()
    {
        navPath = new NavMeshPath();
    }

    // Update is called once per frame
    void Update()
    {
        if (iD == "Carnivore")
        {
            destination = gameObject.GetComponent<Carnivore>().NavA;
        }
        refreshPathCountdown -= Time.deltaTime;
        if (refreshPathCountdown <= 0f)
        {
            refreshPathCountdown = refreshPathDuration;
            RefreshPath();
        }

        UpdateFollow();
    }

    private void UpdateFollow()
    {
        if (followPathIndex >= navPath.corners.Length) return;

        Vector3 followDestination = navPath.corners[followPathIndex];

        followDestination.y = transform.position.y;
        if ((followDestination - transform.position).magnitude <= 0.1f)
        {
            followPathIndex++;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, followDestination, speed * Time.deltaTime);
        }
    }

    private void RefreshPath()
    {
        Vector3 originPos = transform.position;

        Vector3 destinationPos = destination.position;

        bool pathFound = NavMesh.CalculatePath(originPos, destinationPos, NavMesh.AllAreas, navPath);
        if (pathFound)
        {
            followPathIndex = 1;
            Debug.Log("Path Found");
            
        }
        else
        {
            Debug.Log("Path Not Found");
        }
    }

    private void OnDrawGizmos()
    {
        if (null == navPath) return;
        foreach (Vector3 point in navPath.corners)
        {
            Gizmos.DrawSphere(point, 0.5f);
        }
    }
}
