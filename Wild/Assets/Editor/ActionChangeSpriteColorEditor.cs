using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[CustomEditor(typeof(ActionChangeSpriteColor))]
public class ActionChangeSpriteColorEditor : ActionEntityEditor
{
    private ActionChangeSpriteColor script;

    public override void OnEnable()
    {
        base.OnEnable();
        script = (ActionChangeSpriteColor)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(5);

        EditorGUI.BeginChangeCheck();

        GUILayout.BeginVertical("Box");
        GUILayout.Label("Couleur");

        script.currentSpriteColor = EditorGUILayout.ColorField("Couleur : ", script.currentSpriteColor);

        GUILayout.EndVertical();

        if (EditorGUI.EndChangeCheck())
        {
            OnValidate();
        }
    }

    protected override void OnValidate()
    {
        base.OnValidate();

        string name = "CHANGE COLOR";

        if (string.Compare(script.name, "") != 0)
        {
            name = " " + name;
        }

        name += " TO " + ColorUtility.ToHtmlStringRGBA(script.currentSpriteColor);

        script.name += name;
    }
}