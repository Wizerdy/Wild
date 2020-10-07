using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
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

    public void MoveTo(Vector3 dir, float speed) {
        transform.position = new Vector3(
            transform.position.x + dir.x * speed,
            transform.position.y + dir.y * speed,
            transform.position.y + dir.z * speed
        );
    }

    public void MoveToDestination(Vector3 dest) {
        transform.position = dest;
    }

    public void Follow(GameObject go) { }
}
