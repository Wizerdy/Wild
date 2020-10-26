using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Vector2 gameZoneSize;
    public Vector3 startPosition;
    private Camera cam;
    private float height;
    private float width;

    private Entity cameraEntity;
    public string entityToFollow;

    void Start()
    {
        cameraEntity = GetComponent<Entity>();

        cam = GetComponent<Camera>();
        height = 2f * cam.orthographicSize;
        width = height * cam.aspect;
        //startPosition = transform.position;

        if(cameraEntity != null)
        {
            cameraEntity.FollowLock(EntitiesManager.FindEntity(entityToFollow).gameObject);
            cameraEntity.CopyMovementValues(EntitiesManager.FindEntity(entityToFollow));
            cameraEntity.acceleration *= 3f;
            cameraEntity.friction *= 2f;
            cameraEntity.turnFriction *= 2f;
        }
    }

    void Update()
    {
        EntitiesManager.DebugEntities();
        ZoneCollision();
    }

    public void ZoneCollision()
    {
        if (transform.position.x + width / 2 > startPosition.x + gameZoneSize.x / 2)
            transform.position = new Vector3(startPosition.x + gameZoneSize.x / 2 - width / 2, transform.position.y, transform.position.z);
        else if (transform.position.x - width / 2 < startPosition.x - gameZoneSize.x / 2)
            transform.position = new Vector3(startPosition.x - gameZoneSize.x / 2 + width / 2, transform.position.y, transform.position.z);

        if (transform.position.z + height / 2 > startPosition.z + gameZoneSize.y / 2) // Bordure haute
            transform.position = new Vector3(transform.position.x, transform.position.y, startPosition.z + gameZoneSize.y / 2 - height / 2);
        else if (transform.position.z - height / 2 < startPosition.z - gameZoneSize.y / 2) // Bordure basse
            transform.position = new Vector3(transform.position.x, transform.position.y, startPosition.z - gameZoneSize.y / 2 + height / 2);
    }
}
