using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[CustomEditor(typeof(ActionChangeSpeed))]
public class ActionChangeSpeedEditor : ActionEntityEditor
{
    private ActionChangeSpeed script;

    public override void OnEnable()
    {
        base.OnEnable();
        script = (ActionChangeSpeed)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(5);

        EditorGUI.BeginChangeCheck();

        GUILayout.BeginVertical("Box");
        GUILayout.Label("Vitesse maximale");

        script.currentSpeed = EditorGUILayout.IntField("Vitesse : ", script.currentSpeed);

        GUILayout.EndVertical();

        if (EditorGUI.EndChangeCheck())
        {
            OnValidate();
        }
    }

    protected override void OnValidate()
    {
        base.OnValidate();

        string name = "CHANGE SPEED";

        if (string.Compare(script.name, "") != 0)
        {
            name = " " + name;
        }

        name += " TO " + script.currentSpeed;

        script.name += name;
    }
}