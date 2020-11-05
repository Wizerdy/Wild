using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[CustomEditor(typeof(ActionChangeAlpha))]
public class ActionChangeAlphaEditor : ActionEntityEditor
{
    private ActionChangeAlpha script;

    public override void OnEnable()
    {
        base.OnEnable();
        script = (ActionChangeAlpha)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(5);

        EditorGUI.BeginChangeCheck();

        GUILayout.BeginVertical("Box");
        GUILayout.Label("Opacité");

        script.currentAlpha = (byte)EditorGUILayout.Slider("Alpha : ", script.currentAlpha, 0, 255);

        GUILayout.EndVertical();

        if (EditorGUI.EndChangeCheck())
        {
            OnValidate();
        }
    }

    protected override void OnValidate()
    {
        base.OnValidate();

        string name = "CHANGE ALPHA";

        if(string.Compare(script.name, "") != 0)
        {
            name = " " + name;
        }

        name += " TO " + script.currentAlpha;

        script.name += name;
    }
}
