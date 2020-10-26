using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[CustomEditor(typeof(ActionMoveLerp))]
public class ActionMoveLerpEditor : ActionEntityEditor {
    private ActionMoveLerp script;

    public override void OnEnable() {
        base.OnEnable();
        script = (ActionMoveLerp)target;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        GUILayout.Space(5);

        EditorGUI.BeginChangeCheck();

        GUILayout.BeginVertical("box");
        GUILayout.Label("Destination");

        GUILayout.BeginVertical("box");

        script.destinationCurrentTab = GUILayout.Toolbar(script.destinationCurrentTab, new string[] { "GameObject", "Position" });
        switch(script.destinationCurrentTab) {
            case 0:
                script.goDestination = (Transform)EditorGUILayout.ObjectField(script.goDestination, typeof(Transform), true);
                break;
            case 1:
                script.vectorDestination = EditorGUILayout.Vector3Field("", script.vectorDestination);
                break;
        }

        GUILayout.EndVertical();

        script.time = EditorGUILayout.FloatField("Time", script.time);
        script.steps = EditorGUILayout.IntField("Steps", script.steps);

        GUILayout.EndVertical();

        if(EditorGUI.EndChangeCheck()) {
            OnValidate();
        }
    }

    protected override void OnValidate() {
        base.OnValidate();

        string name = "MOVE LERP";

        if(String.Compare(script.name, "") != 0) {
            name = " " + name;
        }

        if (script.destinationCurrentTab == 0 && script.goDestination != null) { name += " TO " + script.goDestination.name; }
        else if (script.destinationCurrentTab == 1) { name += " TO " + script.vectorDestination; }

        script.name += name;
    }
}
