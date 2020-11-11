using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[CustomEditor(typeof(ActionPause))]
public class ActionPauseEditor : ActionEntityEditor {
    public override void OnInspector() {
        base.OnInspector();
        GUILayout.Space(SPACE);

        GUILayout.BeginVertical("Box");
        GUILayout.Label("Pause");

        serializedObject.FindProperty("currentTime").PropertyField();

        GUILayout.EndVertical();
    }

    protected override void OnValidate() {
        base.OnValidate();

        ActionPause script = target as ActionPause;
        if (script == null) { return; }

        string name = "PAUSE";

        if (string.Compare(script.name, "") != 0) {
            name = " " + name;
        }

        name += " DURING " + script.currentTime;

        script.name += name + "s";
    }
}