using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ActionMoveLerp))]
public class ActionMoveLerpEditor : Editor {
    ActionMoveLerp script;

    public void OnEnable() {
        script = (ActionMoveLerp)target;
    }

    public override void OnInspectorGUI() {
        GUILayout.BeginVertical("box");
        GUILayout.Label("Yay !");

        GUILayout.BeginVertical("box");

        script.currentTab = GUILayout.Toolbar(script.currentTab, new string[] { "GameObject", "Entity" });
        switch(script.currentTab) {
            case 0:

                break;
            case 1:
                break;
        }
        GUILayout.EndVertical();

        GUILayout.EndVertical();

        GUILayout.Space(20);
        base.OnInspectorGUI();
    }
}
