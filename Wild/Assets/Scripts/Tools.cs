using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
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
#if UNITY_EDITOR
    public static void PropertyField(this SerializedProperty prop) {
        EditorGUILayout.PropertyField(prop);
    }

#endif
    #region Actions

    public static void ChangeAlphaMaterial(GameObject obj, byte alpha) {
        if (obj.GetComponent<Renderer>() != null) {
            Renderer renderer = obj.GetComponent<Renderer>();
            Color32 col = renderer.material.GetColor("_Color");
            col.a = alpha;
            renderer.material.SetColor("_Color", col);
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

    public static void ChangeAnim(GameObject obj, Animation anim)
    {
        obj.GetComponent<Animation>().clip = anim.clip;
    }

    #endregion
}
