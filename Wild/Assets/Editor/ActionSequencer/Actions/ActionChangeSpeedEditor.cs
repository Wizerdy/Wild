using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[CustomEditor(typeof(ActionChangeSpeed))]
public class ActionChangeSpeedEditor : ActionEntityEditor {
    public override void OnInspector() {
        base.OnInspector();
        GUILayout.Space(SPACE);

        GUILayout.BeginVertical("Box");
        GUILayout.Label("Vitesse maximale");

        serializedObject.FindProperty("currentSpeed").PropertyField();

        GUILayout.EndVertical();
    }

    protected override void OnValidate() {
        base.OnValidate();

        ActionChangeSpeed script = target as ActionChangeSpeed;
        if (script == null) { return; }

        string name = "CHANGE SPEED";

        if (string.Compare(script.name, "") != 0) {
            name = " " + name;
        }

        name += " TO " + script.currentSpeed;

        script.name += name;
    }
}