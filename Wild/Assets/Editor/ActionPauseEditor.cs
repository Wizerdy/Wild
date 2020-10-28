using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[CustomEditor(typeof(ActionPause))]
public class ActionPauseEditor : ActionEntityEditor
{
    private ActionPause script;

    public override void OnEnable()
    {
        base.OnEnable();
        script = (ActionPause)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(5);

        EditorGUI.BeginChangeCheck();

        GUILayout.BeginVertical("Box");
        GUILayout.Label("Pause");

        script.currentTime = EditorGUILayout.FloatField("Temps : ", script.currentTime);

        GUILayout.EndVertical();

        if (EditorGUI.EndChangeCheck())
        {
            OnValidate();
        }
    }

    protected override void OnValidate()
    {
        base.OnValidate();

        string name = "PAUSE";

        if (string.Compare(script.name, "") != 0)
        {
            name = " " + name;
        }

        name += " DURING " + script.currentTime;

        script.name += name + "s";
    }
}