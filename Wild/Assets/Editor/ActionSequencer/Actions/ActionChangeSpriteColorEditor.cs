using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[CustomEditor(typeof(ActionChangeSpriteColor))]
public class ActionChangeSpriteColorEditor : ActionEntityEditor {
    public override void OnInspector() {
        base.OnInspector();
        GUILayout.Space(SPACE);

        GUILayout.BeginVertical("Box");
        GUILayout.Label("Couleur");

        serializedObject.FindProperty("currentSpriteColor").PropertyField();

        GUILayout.EndVertical();
    }

    protected override void OnValidate() {
        base.OnValidate();

        ActionChangeSpriteColor script = target as ActionChangeSpriteColor;
        if (script == null) { return; }

        string name = "CHANGE COLOR";

        if (string.Compare(script.name, "") != 0) {
            name = " " + name;
        }

        name += " TO " + ColorUtility.ToHtmlStringRGBA(script.currentSpriteColor);

        script.name += name;
    }
}