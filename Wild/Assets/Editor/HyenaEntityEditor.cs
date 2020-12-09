using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HyenaEntity))]
[CanEditMultipleObjects]
public class HyenaEntityEditor : Editor {
    private HyenaEntity script;

    private void OnEnable() {
        script = (HyenaEntity)target;
    }

    private void OnSceneGUI() {
        Handles.color = Color.red;
        for (int i = 0; i < script.patrolPoints.Length; i++) {
            Handles.DrawDottedLine(script.patrolPoints[i].ConvertTo3D(), script.patrolPoints[(i + 1) % script.patrolPoints.Length].ConvertTo3D(), 2f);
            script.patrolPoints[i] = Handles.PositionHandle(script.patrolPoints[i].ConvertTo3D(), Quaternion.identity).ConvertTo2D();
        }
    }
}
