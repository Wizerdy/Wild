using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[CustomEditor(typeof(ActionReset))]
public class ActionResetEditor : ActionCameraEditor {
    private ActionReset script;

    public override void OnEnable() {
        base.OnEnable();
        script = (ActionReset)target;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        serializedObject.Update();

        GUILayout.Space(5);

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.BeginVertical("box");
        GUILayout.Label("Durée et framerate");

        SerializedProperty transitionProperty = serializedObject.FindProperty("transition");
        EditorGUILayout.PropertyField(transitionProperty);

        GUILayout.EndVertical();

        if (EditorGUI.EndChangeCheck()) {
            OnValidate();
        }

        serializedObject.ApplyModifiedProperties();
    }

    protected override void OnValidate() {
        base.OnValidate();

        string name = "RESET";

        if (String.Compare(script.name, "") != 0) {
            name = " " + name;
        }

        script.name += name + " " + script.cam.name;
    }
}
