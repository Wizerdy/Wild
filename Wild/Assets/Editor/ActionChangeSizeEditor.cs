using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[CustomEditor(typeof(ActionChangeSize))]
public class ActionChangeSizeEditor : ActionEntityEditor
{
    private ActionChangeSize script;

    public override void OnEnable()
    {
        base.OnEnable();
        script = (ActionChangeSize)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(5);

        EditorGUI.BeginChangeCheck();

        GUILayout.BeginVertical("Box");
        GUILayout.Label("Échelle");

        script.currentSize = EditorGUILayout.IntField("Taille : ", script.currentSize);

        GUILayout.EndVertical();

        if (EditorGUI.EndChangeCheck())
        {
            OnValidate();
        }
    }

    protected override void OnValidate()
    {
        base.OnValidate();

        string name = "CHANGE SIZE";

        if (string.Compare(script.name, "") != 0)
        {
            name = " " + name;
        }

        name += " TO " + script.currentSize;

        script.name += name;
    }
}