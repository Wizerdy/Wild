using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Action))]
public class ActionEditor : Editor {
    private Action script;

    public virtual void OnEnable() {
        script = (Action)target;
    }

    public override void OnInspectorGUI() {
        GUILayout.BeginVertical("box");
        GUILayout.Label("Action system");
        EditorGUILayout.Space(2);
        script.waitType = (Action.WaitType)EditorGUILayout.EnumPopup("Wait type", script.waitType);

        if(script.waitType == Action.WaitType.TIME || script.waitType == Action.WaitType.BOTH) {
            script.timeToWait = EditorGUILayout.FloatField("Time to wait", script.timeToWait);
        }

        GUILayout.EndVertical();
    }

    protected virtual void OnValidate() {
        script.name = "";
    }
}
