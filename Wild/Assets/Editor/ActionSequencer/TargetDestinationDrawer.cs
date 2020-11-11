using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer((typeof(TargetDestination)))]
public class TargetDestinationDrawer : PropertyDrawer {
    public const float Y_MARGINS = 2f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        position.y += Y_MARGINS;

        Rect boxPosition = position;
        boxPosition.height -= Y_MARGINS * 2f;

        GUI.Box(boxPosition, GUIContent.none);
        position.height = EditorGUIUtility.singleLineHeight;
        position.x += EditorGUIUtility.standardVerticalSpacing;
        position.width -= EditorGUIUtility.standardVerticalSpacing * 2f;

        SerializedProperty targetTypeProperty = property.FindPropertyRelative("targetType");

        position.y += EditorGUIUtility.standardVerticalSpacing;
        Rect buttonPosition = position;
        buttonPosition.width /= 2;
        buttonPosition.height = EditorGUIUtility.singleLineHeight;

        int typeIndex = targetTypeProperty.intValue;

        if (GUI.Button(buttonPosition, "Vector")) {
            typeIndex = 0;
        }

        buttonPosition.x += buttonPosition.width;
        if (GUI.Button(buttonPosition, "Transform")) {
            typeIndex = 1;
        }

        targetTypeProperty.intValue = typeIndex;
        TargetDestination.DESTINATION_TARGET_TYPE targetType = (TargetDestination.DESTINATION_TARGET_TYPE)typeIndex;

        switch (targetType) {
            case TargetDestination.DESTINATION_TARGET_TYPE.VECTOR:
                position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(position, property.FindPropertyRelative("vector"));
                break;

            case TargetDestination.DESTINATION_TARGET_TYPE.TRANSFORM:
                position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(position, property.FindPropertyRelative("transform"));
                break;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + Y_MARGINS) * 2f;
    }
}
