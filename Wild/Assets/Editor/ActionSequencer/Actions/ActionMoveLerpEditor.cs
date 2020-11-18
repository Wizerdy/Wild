using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[CustomEditor(typeof(ActionMoveLerp))]
public class ActionMoveLerpEditor : ActionEntityEditor {
    public override void OnInspector() {
        base.OnInspector();
        GUILayout.Space(SPACE);

        GUILayout.BeginVertical("box");
        GUILayout.Label("Destination");

        serializedObject.FindProperty("targDestination").PropertyField();

        //GUILayout.BeginVertical("box");

        //SerializedProperty destinationCurrentTabProperty = serializedObject.FindProperty("destinationCurrentTab");

        //destinationCurrentTabProperty.intValue = GUILayout.Toolbar(destinationCurrentTabProperty.intValue, new string[] { "GameObject", "Position" });
        //switch (destinationCurrentTabProperty.intValue) {
        //    case 0:
        //        serializedObject.FindProperty("goDestination").PropertyField();
        //        break;
        //    case 1:
        //        serializedObject.FindProperty("vectorDestination").PropertyField();
        //        break;
        //}

        //script.destinationCurrentTab = GUILayout.Toolbar(script.destinationCurrentTab, new string[] { "GameObject", "Position" });
        //switch (script.destinationCurrentTab) {
        //    case 0:
        //        script.goDestination = (Transform)EditorGUILayout.ObjectField(script.goDestination, typeof(Transform), true);
        //        break;
        //    case 1:
        //        script.vectorDestination = EditorGUILayout.Vector3Field("", script.vectorDestination);
        //        break;
        //}

        //GUILayout.EndVertical();

        serializedObject.FindProperty("time").PropertyField();

        GUILayout.EndVertical();
    }

    protected override void OnValidate() {
        base.OnValidate();

        ActionMoveLerp script = target as ActionMoveLerp;
        if (script == null) { return; }

        string name = "MOVE LERP";

        if (!String.IsNullOrWhiteSpace(script.name)) { name = " " + name; }

        //if (script.destinationCurrentTab == 0 && script.goDestination != null) { name += " TO " + script.goDestination.name; }
        //else if (script.destinationCurrentTab == 1) { name += " TO " + script.vectorDestination; }

        string destination = script.targDestination.DestinationName();
        if(!String.IsNullOrWhiteSpace(destination)) { name += " " + destination; }

        script.name += name;
    }
}
