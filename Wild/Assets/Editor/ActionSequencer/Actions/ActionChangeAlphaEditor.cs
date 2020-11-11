using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[CustomEditor(typeof(ActionChangeAlpha))]
public class ActionChangeAlphaEditor : ActionEntityEditor {
    public override void OnInspector() {
        base.OnInspector();
        GUILayout.Space(SPACE);

        GUILayout.BeginVertical("Box");
        GUILayout.Label("Opacité");

        serializedObject.FindProperty("currentAlpha").PropertyField();

        GUILayout.EndVertical();
    }

    protected override void OnValidate() {
        base.OnValidate();

        ActionChangeAlpha script = target as ActionChangeAlpha;
        if (script == null) { return; }

        string name = "CHANGE ALPHA";

        if (string.Compare(script.name, "") != 0) {
            name = " " + name;
        }

        name += " TO " + script.currentAlpha;

        script.name += name;
    }
}
