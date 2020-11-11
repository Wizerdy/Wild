using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ActionEntity))]
public class ActionEntityEditor : ActionEditor {
    public override void OnInspector() {
        SerializedProperty targEntityProperty = serializedObject.FindProperty("targEntity");
        EditorGUILayout.PropertyField(targEntityProperty);

        EditorGUILayout.Space(SPACE);
    }

    protected override void OnValidate() {
        base.OnValidate();

        ActionEntity script = target as ActionEntity;
        if(script == null) { return; }

        script.name += script.targEntity.EntityName();
    }
}
