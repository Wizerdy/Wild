using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ActionCamera))]
public class ActionCameraEditor : ActionEditor
{
    private ActionCamera script;

    public override void OnEnable()
    {
        base.OnEnable();
        script = (ActionCamera)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(5);

        EditorGUI.BeginChangeCheck();

        GUILayout.BeginVertical("box");
        GUILayout.Label("Camera");

        script.cam = ((CameraManager)EditorGUILayout.ObjectField(script.cam, typeof(CameraManager), true));

        GUILayout.EndVertical();

        if (EditorGUI.EndChangeCheck())
        {
            OnValidate();
        }
    }

    protected override void OnValidate()
    {
        base.OnValidate();

        string name = "";

        script.name += name;
    }
}
