using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ActionCamera))]
public class ActionCameraEditor : ActionEditor {
    public override void OnInspector() {
        GUILayout.Space(SPACE);

        GUILayout.BeginVertical("box");
        GUILayout.Label("Camera");

        SerializedProperty camProperty = serializedObject.FindProperty("cam");
        EditorGUILayout.PropertyField(camProperty);
        //script.cam = ((CameraManager)EditorGUILayout.ObjectField(script.cam, typeof(CameraManager), true));

        GUILayout.EndVertical();
    }

    protected override void OnValidate() {
        base.OnValidate();

        ActionCamera script = target as ActionCamera;
        if (script == null) { return; }

        string name = script.cam.gameObject.name;

        script.name += name;
    }
}
