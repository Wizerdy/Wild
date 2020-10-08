using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Vector2 gameZoneSize;
    private Vector2 startPosition;
    private Camera cam;
    private float height;
    private float width;

    void Start()
    {
        cam = GetComponent<Camera>();
        height = 2f * cam.orthographicSize;
        width = height * cam.aspect;
        startPosition = transform.position;
    }

    void Update()
    {
        ZoneCollision();
    }

    public void ZoneCollision()
    {
        if (transform.position.x + width / 2 > startPosition.x + gameZoneSize.x / 2) // Bordure droite
            transform.position = new Vector2(startPosition.x + gameZoneSize.x / 2, transform.position.y);
        else if (transform.position.x - width / 2 < startPosition.x - gameZoneSize.x / 2) // Bordure gauche
            transform.position = new Vector2(startPosition.x - gameZoneSize.x / 2, transform.position.y);

        if (transform.position.y + height / 2 > startPosition.y + gameZoneSize.y / 2) // Bordure haute
            transform.position = new Vector2(transform.position.x, startPosition.y + gameZoneSize.y / 2);
        else if (transform.position.y - height / 2 < startPosition.y - gameZoneSize.y / 2) // Bordure basse
            transform.position = new Vector2(transform.position.x, startPosition.y - gameZoneSize.x / 2);
    }
}
