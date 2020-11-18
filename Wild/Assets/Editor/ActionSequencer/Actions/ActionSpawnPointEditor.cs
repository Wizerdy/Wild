using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[CustomEditor(typeof(ActionSpawnPoint))]
public class ActionSpawnPointEditor : ActionEntityEditor {
    public override void OnInspector() {
        base.OnInspector();
        GUILayout.Space(SPACE);

        GUILayout.BeginVertical("Box");
        GUILayout.Label("SpawnPoint");

        serializedObject.FindProperty("position").PropertyField();

        GUILayout.EndVertical();
    }

    protected override void OnValidate() {
        base.OnValidate();

        ActionSpawnPoint script = target as ActionSpawnPoint;
        if (script == null) { return; }

        string name = "CHANGE SPAWNPOINT";

        if (string.Compare(script.name, "") != 0) {
            name = " " + name;
        }

        name += " TO " + script.position;

        script.name += name;
    }
}