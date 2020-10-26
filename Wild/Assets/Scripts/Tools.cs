using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Tools
{
    public static Vector3 ConvertTo3D(this Vector2 vector) {
        return new Vector3(vector.x, 0, vector.y);
    }

    public static Vector3 ConvertTo3D(this Vector2 vector, float y) {
        return new Vector3(vector.x, y, vector.y);
    }

    public static Vector2 ConvertTo2D(this Vector3 vector) {
        return new Vector2(vector.x, vector.z);
    }

    public static Vector3 OverwriteY(this Vector3 vector, float y) {
        return new Vector3(vector.x, y, vector.z);
    }

    public static void LoadScene(int num) {
        EntitiesManager.ClearEntities();
        SceneManager.LoadScene(num);
    }

    public static void ChangeAlphaMaterial(GameObject obj, byte alpha)
    {
        if (obj.GetComponent<Renderer>() != null)
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            Color32 col = renderer.material.GetColor("_BaseColor");
            col.a = alpha;
            renderer.material.SetColor("_BaseColor", col);
        }
    }

    public static void ChangeSpeed(GameObject obj, int speed)
    {
        if (obj.GetComponent<Entity>() != null)
        {
            obj.GetComponent<Entity>().speedMax = speed;
        }
    }

    public static void ChangeSize(GameObject obj, int size)
    {
        obj.transform.localScale = new Vector3(size, size, size);
    }

    public static void ChangeSpriteColor(GameObject obj, Color c)
    {
        if (obj.GetComponent<SpriteRenderer>() != null)
        {
            obj.GetComponent<SpriteRenderer>().color = c;
        }
    }

    public static void ChangeMaterialColor(GameObject obj, Color c)
    {
        if (obj.GetComponent<Material>() != null)
        {
            obj.GetComponent<Material>().color = c;
        }
    }
}
