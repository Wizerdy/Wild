using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManagerOld : MonoBehaviour
{
    [System.Serializable]
    public struct CameraSettings
    {
        public Vector3 startPosition;
        public Vector2 gameZoneSize;

        public CameraSettings(Vector2 size, Vector3 pos)
        {
            gameZoneSize = size;
            startPosition = pos;
        }
    }

    public Vector2 gameZoneSize;
    public Vector3 startPosition;
    private Camera cam;
    private float height;
    private float width;
    private CameraSettings[] arrayOfCamera;
    private BoxCollider2D[] arrayOfBoxes;
    private Vector3 followDelta;
    private Quaternion baseRotation;

    [HideInInspector] public float percentage;
    [HideInInspector] public Entity cameraEntity;
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
            //cameraEntity.acceleration *= 3f;
            //cameraEntity.friction *= 2f;
            //cameraEntity.turnFriction *= 2f;
        }
    }

    void Update()
    {
        //EntitiesManager.DebugEntities();
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

    private Vector3 _CameraClampBoundsPosition(Vector3 position, Rect rect)
    {
        float zOffset = position.z - cam.transform.position.z;
        Vector3 bottomLeft = cam.ScreenToWorldPoint(new Vector3(0f, 0f, zOffset));
        Vector3 topRight = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight, zOffset));
        Vector2 screenSize = new Vector2(topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);

        if (position.x > rect.xMax - (screenSize.x / 2f))
        {
            position.x = rect.xMax - (screenSize.x / 2f);
        }
        if (position.x < rect.xMin - (screenSize.x / 2f))
        {
            position.x = rect.xMin - (screenSize.x / 2f);
        }

        if (position.x > rect.yMax - (screenSize.x / 2f))
        {
            position.x = rect.yMax - (screenSize.x / 2f);
        }
        if (position.x > rect.yMax - (screenSize.x / 2f))
        {
            position.x = rect.yMax - (screenSize.x / 2f);
        }

        return position;
    }

    public void ChangeIndex(int index)
    {
        startPosition = arrayOfCamera[index].startPosition;
        gameZoneSize = arrayOfCamera[index].gameZoneSize;
    }

    public void Zoom(Vector3 destination, float time, int steps, float percentage)
    {
        cameraEntity.DoMoveLerp(Vector3.Lerp(cam.transform.position, destination, percentage), time, steps);
        this.percentage = percentage;
    }

    public void ResetCameraToBase(float time, int steps)
    {
        Entity entity = EntitiesManager.FindEntity(entityToFollow);
        cameraEntity.DoMoveLerp(followDelta - entity.transform.position, time, steps);
        cam.transform.rotation = baseRotation;
    }
}
