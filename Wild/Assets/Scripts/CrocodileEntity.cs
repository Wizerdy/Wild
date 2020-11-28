using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrocodileEntity : AnimalEntity
{
    public enum AlertState
    {
        NONE, ALERT, HUNT
    }

    [Header("Field of view")]
    public int raycastNumber = 2;
    public float fieldOfView = 90f;
    public float distanceOfView = 10f;

    [Header("Chase")]
    public AlertState alertState;
    public string preyGroupId;
    //private GameObject prey;

    private void EnterCollision(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        foreach (var hitCollider in hitColliders)
        {
            for (int i = 0; i < hitCollider.GetComponent<Entity>().entityGroup.Length; i++)
            {
                if (hitCollider.GetComponent<Entity>().entityGroup[i] == preyGroupId)
                {

                }
            }
        }
    }

    private void ChangeAlert()
    {
        if (alertState < AlertState.HUNT)
        {
            alertState += 1;
        } else {
            alertState = AlertState.NONE;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
}
