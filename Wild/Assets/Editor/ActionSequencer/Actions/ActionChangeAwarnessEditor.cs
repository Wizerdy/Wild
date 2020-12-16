using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[CustomEditor(typeof(ActionChangeAwarness))]
public class ActionChangeAwarnessEditor : ActionEntityEditor {
    public override void OnInspector() {
        base.OnInspector();
        GUILayout.Space(SPACE);

        GUILayout.BeginVertical("Box");
        GUILayout.Label("Awarness");

        SerializedProperty awarnessProperty = serializedObject.FindProperty("awarness");
        awarnessProperty.PropertyField();

        HyenaEntity.Awarness awarness = (HyenaEntity.Awarness)awarnessProperty.enumValueIndex;
        if (awarness == HyenaEntity.Awarness.CHASING || awarness == HyenaEntity.Awarness.SUSPICIOUS) {
            serializedObject.FindProperty("prey").PropertyField();
        }

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
