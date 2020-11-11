using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[CustomEditor(typeof(ActionReset))]
public class ActionResetEditor : ActionCameraEditor {
    public override void OnInspector() {
        base.OnInspector();
        GUILayout.Space(SPACE);

        EditorGUILayout.BeginVertical("box");
        GUILayout.Label("Durée et framerate");

        SerializedProperty transitionProperty = serializedObject.FindProperty("transition");
        EditorGUILayout.PropertyField(transitionProperty);

        GUILayout.EndVertical();
    }

    protected override void OnValidate() {
        base.OnValidate();

        ActionReset script = target as ActionReset;
        if (script == null) { return; }

        string name = "RESET";

        if (String.IsNullOrWhiteSpace(script.name)) {
            name = " " + name;
        }

        script.name += name + " " + script.cam.name;
    }
}
