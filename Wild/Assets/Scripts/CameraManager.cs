using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Vector3 startPosition;
    public Vector2 gameZoneSize;
    public GameObject gameZoneCollider;
    private CameraSettings[] arrayOfCamera;
    private BoxCollider2D[] arrayOfBoxes;

    private Camera cam;
    private float height;
    private float width;
    private Vector3 followDelta;
    private Quaternion baseRotation;

    [HideInInspector] public Entity cameraEntity;
    [HideInInspector] public float percentage;
    public string entityToFollow;

    [System.Serializable]
    public struct CameraSettings
    {
        public Vector3 startPosition;
        public Vector2 gameZoneSize;

        public CameraSettings (Vector2 size, Vector3 pos)
        {
            gameZoneSize = size;
            startPosition = pos;
        }
    }

    void Start()
    {

        cameraEntity = GetComponent<Entity>();

        cam = GetComponent<Camera>();
        height = 2f * cam.orthographicSize;
        width = height * cam.aspect;
        //startPosition = transform.position;

        arrayOfBoxes = gameZoneCollider.GetComponents<BoxCollider2D>();
        arrayOfCamera = new CameraSettings[arrayOfBoxes.Length];

        for (int i = 0; i < arrayOfBoxes.Length; i++)
        {
            arrayOfCamera[i].startPosition = arrayOfBoxes[i].offset;
            arrayOfCamera[i].gameZoneSize = arrayOfBoxes[i].size;
        }

        baseRotation = transform.rotation;

        //startPosition = arrayOfCamera[Random.Range(0, arrayOfBoxes.Length)].startPosition;
        //gameZoneSize = arrayOfCamera[Random.Range(0, arrayOfBoxes.Length)].gameZoneSize;

        if(cameraEntity != null)
        {
            Entity entity = EntitiesManager.FindEntity(entityToFollow);
            followDelta = cameraEntity.transform.position - entity.transform.position;

            cameraEntity.FollowLock(entity.gameObject, followDelta);
            cameraEntity.CopyMovementValues(entity);
            cameraEntity.acceleration *= 3f;
            cameraEntity.friction *= 2f;
            cameraEntity.turnFriction *= 2f;
        }
    }

    void Update()
    {
        //EntitiesManager.DebugEntities();
        //ZoneCollision();
    }

    private Vector3 _CameraClampBoundsPosition(Vector3 position, Rect rect)
    {
        float zOffset = position.z - cam.transform.position.z;
        Vector3 bottomLeft = cam.ScreenToWorldPoint(new Vector3(0f, 0f, zOffset));
        Vector3 topRight = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight, zOffset));
        Vector2 screenSize = new Vector2(topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);

        if(position.x > rect.xMax - (screenSize.x / 2f)) {
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

    #region Old
    //2D Version
    public void ZoneCollision()
    {
        if (transform.position.x + width / 2 > startPosition.x + gameZoneSize.x / 2)
            transform.position = new Vector3(startPosition.x + gameZoneSize.x / 2 - width / 2, transform.position.y, transform.position.z);
        else if (transform.position.x - width / 2 < startPosition.x - gameZoneSize.x / 2)
            transform.position = new Vector3(startPosition.x - gameZoneSize.x / 2 + width / 2, transform.position.y, transform.position.z);

        if (transform.position.y + height / 2 > startPosition.y + gameZoneSize.y / 2) // Bordure haute
            transform.position = new Vector3(transform.position.x, startPosition.y + gameZoneSize.y / 2 - height / 2, transform.position.z);
        else if (transform.position.y - height / 2 < startPosition.y - gameZoneSize.y / 2) // Bordure basse
            transform.position = new Vector3(transform.position.x, startPosition.y - gameZoneSize.y / 2 + height / 2, transform.position.z);
    }
    #endregion

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
