using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ActionEntity))]
public class ActionEntityEditor : ActionEditor {
    private ActionEntity script;

    public override void OnEnable() {
        base.OnEnable();
        script = (ActionEntity)target;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        GUILayout.Space(5);

        EditorGUI.BeginChangeCheck();

        GUILayout.BeginVertical("box");
        GUILayout.Label("Target entity");

        GUILayout.BeginVertical("box");

        script.entityCurrentTab = GUILayout.Toolbar(script.entityCurrentTab, new string[] { "Entity Id", "GameObject" });
        switch (script.entityCurrentTab) {
            case 0:
                script.entityId = EditorGUILayout.TextField(script.entityId);
                break;
            case 1:
                //script.goDestination = (GameObject)(EditorGUILayout.ObjectField(script.goDestination, typeof(GameObject), true));
                script.entityGo = ((Entity)EditorGUILayout.ObjectField(script.entityGo, typeof(Entity), true));
                break;
        }
        GUILayout.EndVertical();

        GUILayout.EndVertical();

        if (EditorGUI.EndChangeCheck()) {
            OnValidate();
        }
    }

    protected override void OnValidate() {
        base.OnValidate();

        string name = "";

        if (script.entityCurrentTab == 0 && String.Compare(script.entityId, "") != 0) { name = script.entityId; }
        else if (script.entityCurrentTab == 1 && script.entityGo != null) { name = script.entityGo.name; }

        script.name += name;
    }
}
