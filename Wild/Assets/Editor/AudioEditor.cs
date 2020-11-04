using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Sound_Manager))]
public class AudioEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Sound_Manager sm = (Sound_Manager)target;
        if (GUILayout.Button("Add Sound"))
        {
            sm.AddSound();
        }
    }
}
