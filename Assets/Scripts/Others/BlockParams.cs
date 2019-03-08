using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class BlockParams
{
    [SerializeField]
    public GameObject[] prefab;
    [SerializeField]
    public AnimationCurve[] chanceWeight;
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(BlockParams))]
public class HumanPropertyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * (CountElements + (CountElements == 0 ? 1 : 2));
    }

    private const float space = 5;
    private int CountElements;

    public override void OnGUI(Rect rect,
                               SerializedProperty property,
                               GUIContent label)
    {
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        var firstLineRect = new Rect(
            x: rect.x,
            y: rect.y,
            width: rect.width,
            height: EditorGUIUtility.singleLineHeight
        );

        DrawBlockSettings(firstLineRect, property.FindPropertyRelative("prefab"));

        var LineRect = new Rect(
            x: rect.x,
            y: rect.y + EditorGUIUtility.singleLineHeight,
            height: EditorGUIUtility.singleLineHeight,
            width: rect.width
        );

        if (CountElements != 0)
            DrawNameColumn(LineRect, property);

        var ListRect = new Rect(
        x: LineRect.x,
        y: LineRect.y + EditorGUIUtility.singleLineHeight,
        height: EditorGUIUtility.singleLineHeight,
        width: LineRect.width
        );

        for (int i = 0; i < CountElements; i++)
        {
            DrawMainProperties(ListRect, property, i);
            ListRect.y += ListRect.height;
        }
        EditorGUI.indentLevel = indent;
    }

    private void DrawNameColumn(Rect rect, SerializedProperty property)
    {
        GUIContent label = new GUIContent("<b><size=8>  Prefab</size></b>");
        Rect contentPosition = EditorGUI.PrefixLabel(rect, label, GUIStyle.none);
        contentPosition.width = contentPosition.width;
        EditorGUI.LabelField(contentPosition, "<b><size=8>Create Chance</size></b>", GUIStyle.none);
    }

    private void DrawMainProperties(Rect rect, SerializedProperty property, int index)
    {

        GUIContent label = new GUIContent(" ");
        Rect contentPosition = EditorGUI.PrefixLabel(rect, label);
        SerializedProperty serializedProperty;

        Rect prefabPosition = new Rect(
        x: rect.x,
        y: rect.y,
        height: EditorGUIUtility.singleLineHeight,
        width: rect.width - contentPosition.width
        );

        serializedProperty = property.FindPropertyRelative("prefab");
        serializedProperty.arraySize = CountElements;
        DrawProperty(prefabPosition, serializedProperty.GetArrayElementAtIndex(index));

        contentPosition.width = contentPosition.width;

        serializedProperty = property.FindPropertyRelative("chanceWeight");
        serializedProperty.arraySize = CountElements;
        DrawProperty(contentPosition, serializedProperty.GetArrayElementAtIndex(index));
    }

    private void DrawProperty(Rect rect, SerializedProperty property)
    {
        EditorGUI.PropertyField(rect, property, GUIContent.none);
    }

    private void DrawBlockSettings(Rect rect, SerializedProperty BlocksArray)
    {
        var countRect = new Rect(
            x: rect.x,
            y: rect.y,
            height: EditorGUIUtility.singleLineHeight,
            width: rect.width
        );
        var label = new GUIContent("Count blocks is ");
        BlocksArray.arraySize = EditorGUI.IntField(countRect, label, BlocksArray.arraySize);

        CountElements = BlocksArray.arraySize;
    }
}
#endif