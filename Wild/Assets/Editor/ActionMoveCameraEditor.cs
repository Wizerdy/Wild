using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[CustomEditor(typeof(ActionMoveCamera))]
public class ActionMoveCameraEditor : ActionCameraEditor
{
    private ActionMoveCamera script;

    public override void OnEnable()
    {
        base.OnEnable();
        script = (ActionMoveCamera)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        GUILayout.Space(5);

        EditorGUI.BeginChangeCheck();

        GUILayout.BeginVertical("box");
        GUILayout.Label("Destination");

        SerializedProperty transitionProperty = serializedObject.FindProperty("transition");
        EditorGUILayout.PropertyField(transitionProperty);

        script.camTemp = (Camera)EditorGUILayout.ObjectField(script.camTemp, typeof(Camera), true);

        GUILayout.EndVertical();

        if (EditorGUI.EndChangeCheck())
        {
            OnValidate();
        }

        serializedObject.ApplyModifiedProperties();
    }

    protected override void OnValidate()
    {
        base.OnValidate();

        string name = script.cam.name + " MOVE TO";

        if (String.Compare(script.name, "") != 0)
        {
            name = " " + script.camTemp.name + " " + name;
        }

        script.name += name;
    }
}
