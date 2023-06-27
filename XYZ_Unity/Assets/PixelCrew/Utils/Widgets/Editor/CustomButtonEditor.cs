using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(CustomButton), true)]
[CanEditMultipleObjects]


public class CustomButtonEditor : ButtonEditor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_normal"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_pressed"));
        serializedObject.ApplyModifiedProperties();

        base.OnInspectorGUI();
    }
}
