using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ActionPlaySoundObject))]
public class ActionPlaySoundObjectEditor : ActionEditor {
    public override void OnInspector() {
        GUILayout.Space(SPACE);

        GUILayout.BeginVertical("box");
        GUILayout.Label("Sound object");

        SerializedProperty so = serializedObject.FindProperty("soundObject");
        EditorGUILayout.ObjectField(so);

        GUILayout.EndVertical();
    }

    protected override void OnValidate() {
        base.OnValidate();

        ActionEntity script = target as ActionEntity;
        if (script == null) { return; }

        script.name += script.targEntity.EntityName();
    }
}
