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
}
