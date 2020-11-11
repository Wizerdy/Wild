using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[CustomEditor(typeof(ActionZoom))]
public class ActionZoomEditor : ActionCameraEditor {
    public override void OnInspector() {
        base.OnInspector();
        GUILayout.Space(SPACE);

        EditorGUILayout.BeginVertical("box");
        GUILayout.Label("Destination");

        serializedObject.FindProperty("targDestination").PropertyField();

        //script.destinationCurrentTab = GUILayout.Toolbar(script.destinationCurrentTab, new string[] { "GameObject", "Position" });
        //switch (script.destinationCurrentTab) {
        //    case 0:
        //        script.goDestination = (Transform)EditorGUILayout.ObjectField(script.goDestination, typeof(Transform), true);
        //        break;
        //    case 1:
        //        script.vectorDestination = EditorGUILayout.Vector3Field("", script.vectorDestination);
        //        break;
        //}

        serializedObject.FindProperty("transition").PropertyField();

        serializedObject.FindProperty("percentage").PropertyField();

        EditorGUILayout.EndVertical();
    }

    protected override void OnValidate() {
        base.OnValidate();

        ActionZoom script = target as ActionZoom;
        if (script == null) { return; }

        string name = "";
        if(!String.IsNullOrWhiteSpace(script.name)) {
            name += " ";
        }

        name += "ZOOM";

        string destination = script.targDestination.DestinationName();
        if (!String.IsNullOrWhiteSpace(destination)) {
            name += " " + destination;
        }

        script.name += name;
    }
}
