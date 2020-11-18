using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HideZone))]
public class HideZoneEditor : Editor
{
    private HideZone script;

    private void OnEnable()
    {
        script = (HideZone)target;
    }

    private void OnSceneGUI()
    {
        //Handles.color = Color.red;
        //Handles.DrawDottedLine(script.exitPoint, script.exitPoint, 2f);
        Handles.DrawGizmos(Camera.main);
        script.exitPoint = Handles.PositionHandle(script.exitPoint, Quaternion.identity);
    }
}