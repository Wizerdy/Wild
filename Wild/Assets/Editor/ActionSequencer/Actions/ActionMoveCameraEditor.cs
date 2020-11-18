using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[CustomEditor(typeof(ActionMoveCamera))]
public class ActionMoveCameraEditor : ActionCameraEditor {
    public override void OnInspector() {
        base.OnInspector();
        GUILayout.Space(SPACE);

        GUILayout.BeginVertical("box");
        GUILayout.Label("Destination");

        serializedObject.FindProperty("transition").PropertyField();

        serializedObject.FindProperty("camTemp").PropertyField();

        GUILayout.EndVertical();
    }

    protected override void OnValidate() {
        base.OnValidate();

        ActionMoveCamera script = target as ActionMoveCamera;
        if (script == null) { return; }

        string name = "MOVE TO";

        if(!String.IsNullOrWhiteSpace(script.name)) { name = " " + name; }

        if (script.camTemp != null) { name += " " + script.camTemp.name; }

        script.name += name;
    }
}
