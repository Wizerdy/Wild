using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Identity")]
    public string entityId;
    public string[] entityGroup;

    private void Awake()
    {
        EntitiesManager.AddEntity(this);
    }

    public void MoveTo(Vector3 dir) { }

    public void MoveToDestination(Vector3 dest) { }

    public void Follow(GameObject go) { }
}
