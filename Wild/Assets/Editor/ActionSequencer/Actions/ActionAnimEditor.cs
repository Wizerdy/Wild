using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[CustomEditor(typeof(ActionChangeAnim))]
public class ActionChangeAnimEditor : ActionEntityEditor {
    public override void OnInspector() {
        base.OnInspector();
        GUILayout.Space(SPACE);

        GUILayout.BeginVertical("Box");
        GUILayout.Label("Animation");

        SerializedProperty animProperty = serializedObject.FindProperty("anim");
        EditorGUILayout.PropertyField(animProperty);

        GUILayout.EndVertical();
    }

    protected override void OnValidate() {
        base.OnValidate();

        ActionChangeAnim script = target as ActionChangeAnim;
        if (script == null) { return; }

        string name = "CHANGE ANIMATION";

        if (string.Compare(script.name, "") != 0) {
            name = " " + name;
        }

        if (script.anim != null) {
            name += " TO " + script.anim.name;
        }

        script.name += name;
    }
}