using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer((typeof(TargetEntity)))]
public class TargetEntityDrawer : PropertyDrawer
{
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
        //EditorGUI.PropertyField(position, targetTypeProperty);
        //TargetEntity.ENTITY_TARGET_TYPE targetType = (TargetEntity.ENTITY_TARGET_TYPE)targetTypeProperty.intValue;
        position.y += EditorGUIUtility.standardVerticalSpacing;

        Rect buttonPosition = position;
        buttonPosition.width /= 2;
        buttonPosition.height = EditorGUIUtility.singleLineHeight;

        int typeIndex = targetTypeProperty.intValue;

        if (GUI.Button(buttonPosition, "Id")) {
            typeIndex = 0;
        }

        buttonPosition.x += buttonPosition.width;
        if(GUI.Button(buttonPosition, "Entity")) {
            typeIndex = 1;
        }

        targetTypeProperty.intValue = typeIndex;
        TargetEntity.ENTITY_TARGET_TYPE targetType = (TargetEntity.ENTITY_TARGET_TYPE)typeIndex;

        switch (targetType) {
            case TargetEntity.ENTITY_TARGET_TYPE.ENTITY:
                position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(position, property.FindPropertyRelative("entityGo"));
                break;

            case TargetEntity.ENTITY_TARGET_TYPE.ENTITY_ID:
                position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(position, property.FindPropertyRelative("entityId"));
                break;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + Y_MARGINS) * 2f;
    }
}
