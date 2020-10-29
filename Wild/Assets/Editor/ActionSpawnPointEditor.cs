using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[CustomEditor(typeof(ActionSpawnPoint))]
public class ActionSpawnPointEditor : ActionEditor
{
    private ActionSpawnPoint script;

    public override void OnEnable()
    {
        base.OnEnable();
        script = (ActionSpawnPoint)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(5);

        EditorGUI.BeginChangeCheck();

        GUILayout.BeginVertical("Box");
        GUILayout.Label("SpawnPoint");

        script.position = EditorGUILayout.Vector2Field("Position : ", script.position);

        GUILayout.EndVertical();

        if (EditorGUI.EndChangeCheck())
        {
            OnValidate();
        }
    }

    protected override void OnValidate()
    {
        base.OnValidate();

        string name = "CHANGE SPAWNPOINT";

        if (string.Compare(script.name, "") != 0)
        {
            name = " " + name;
        }

        name += " TO " + script.position;

        script.name += name;
    }
}