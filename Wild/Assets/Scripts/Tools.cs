using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Tools
{
    public enum IgnoreMode { X, Y, Z }

    public static Vector3 ConvertTo3D(this Vector2 vector) {
        return new Vector3(vector.x, 0, vector.y);
    }

    public static Vector3 ConvertTo3D(this Vector2 vector, float y) {
        return new Vector3(vector.x, y, vector.y);
    }

    public static Vector2 ConvertTo2D(this Vector3 vector) {
        return new Vector2(vector.x, vector.z);
    }

    public static Vector3 Overwrite(this Vector3 vector, float value, IgnoreMode ignore) {
        switch(ignore) {
            case IgnoreMode.X:
                return new Vector3(value, vector.y, vector.z);
            case IgnoreMode.Y:
                return new Vector3(vector.x, value, vector.z);
            case IgnoreMode.Z:
                return new Vector3(vector.x, vector.y, value);
        }

        return vector;
    }

    public static void LoadScene(int num) {
        EntitiesManager.ClearEntities();
        SceneManager.LoadScene(num);
    }

    public static void ChangeAlphaMaterial(GameObject obj, byte alpha) {
        if (obj.GetComponent<Renderer>() != null) {
            Renderer renderer = obj.GetComponent<Renderer>();
            Color32 col = renderer.material.GetColor("_BaseColor");
            col.a = alpha;
            renderer.material.SetColor("_BaseColor", col);
        }
    }
}
