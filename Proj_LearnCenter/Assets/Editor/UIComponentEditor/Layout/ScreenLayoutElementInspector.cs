using UnityEditor;
using System.Collections.Generic;
using UnityEngine;

[CustomEditor(typeof(ScreenLayoutElement))]
[CanEditMultipleObjects]
public class ScreenLayoutElementInspector:Editor
{
    ScreenLayoutElement elem;
    SerializedProperty aspectProperty;
    SerializedProperty maxWidthProperty;
    SerializedProperty maxHeightProperty;
    SerializedProperty widthScreenPercent;
    SerializedProperty heightScreenPercent;

    private void OnEnable()
    {
        elem = target as ScreenLayoutElement;
        aspectProperty = serializedObject.FindProperty("aspect");        
        maxWidthProperty = serializedObject.FindProperty("maxWidth");
        maxHeightProperty = serializedObject.FindProperty("maxHeight");
        widthScreenPercent = serializedObject.FindProperty("ScreenWidthPercent");
        heightScreenPercent = serializedObject.FindProperty("ScreenHeightPercent");
    }

    public override void OnInspectorGUI()
    {
        elem.ignoreLayout = EditorGUILayout.Toggle("Ignore Layout",elem.ignoreLayout);

        EditorGUILayout.PropertyField(aspectProperty, new GUIContent("Aspect"));

        EditorGUILayout.BeginHorizontal();
        elem.minWidth = EditorGUILayout.FloatField("MinWidth",elem.minWidth);
        EditorGUILayout.PropertyField(maxWidthProperty, new GUIContent("MaxWidth"));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        elem.minHeight = EditorGUILayout.FloatField("MinHeight", elem.minHeight);
        EditorGUILayout.PropertyField(maxHeightProperty, new GUIContent("MaxHeight"));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.PropertyField(widthScreenPercent, new GUIContent("ScreenWidthPercent"));
        EditorGUILayout.PropertyField(heightScreenPercent, new GUIContent("ScreenHeightPercent"));
        serializedObject.ApplyModifiedProperties();
    }
    
}


