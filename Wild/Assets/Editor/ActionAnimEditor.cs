using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[CustomEditor(typeof(ActionChangeAnim))]
public class ActionChangeAnimEditor : ActionEntityEditor
{
    private ActionChangeAnim script;

    public override void OnEnable()
    {
        base.OnEnable();
        script = (ActionChangeAnim)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(5);

        EditorGUI.BeginChangeCheck();

        GUILayout.BeginVertical("Box");
        GUILayout.Label("Animation");

        script.anim = (Animation)EditorGUILayout.ObjectField(script.anim, typeof(Animation), true);

        GUILayout.EndVertical();

        if (EditorGUI.EndChangeCheck())
        {
            OnValidate();
        }
    }

    protected override void OnValidate()
    {
        base.OnValidate();

        string name = "CHANGE ANIMATION";

        if (string.Compare(script.name, "") != 0)
        {
            name = " " + name;
        }

        if (script.anim != null)
        {
            name += " TO " + script.anim.name;
        }

        script.name += name;
    }
}