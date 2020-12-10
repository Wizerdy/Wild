using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SnakeEntity))]
public class SnakeEntityEditor : Editor
{
    private SnakeEntity script;

    private void OnEnable()
    {
        script = (SnakeEntity)target;
    }

    private void OnSceneGUI()
    {
        //Handles.color = Color.red;
        //Handles.DrawDottedLine(script.exitPoint, script.exitPoint, 2f);
        Handles.DrawGizmos(Camera.main);
        script.lionTp = Handles.PositionHandle(script.lionTp, Quaternion.identity);

        Handles.color = script.circleColor;
        Handles.DrawSolidDisc(script.gameObject.transform.position, new Vector3(0, 1, 0), script.presenceRadius);
    }
}