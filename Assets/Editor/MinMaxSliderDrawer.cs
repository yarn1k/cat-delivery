using Editor;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(MinMaxSliderAttribute))]
internal sealed class MinMaxSliderAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        bool isVector2Int = property.propertyType == SerializedPropertyType.Vector2Int;
        if (property.propertyType != SerializedPropertyType.Vector2 && !isVector2Int)
        {
            Debug.LogWarning("MinMaxSliderAttribute requires a Vector2 or Vector2Int property");
            return;
        }

        int originalIndentLevel = EditorGUI.indentLevel;
        var attr = (MinMaxSliderAttribute)attribute;

        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        EditorGUI.indentLevel = 0;
        float min = isVector2Int ? property.vector2IntValue.x : property.vector2Value.x;
        float max = isVector2Int ? property.vector2IntValue.y : property.vector2Value.y;
        float fieldWidth = position.width / 4f - 4f;
        float sliderWidth = position.width / 2f;
        position.width = fieldWidth;
        min = EditorGUI.FloatField(position, min);
        position.x += fieldWidth + 4f;
        position.width = sliderWidth;

        EditorGUI.MinMaxSlider(position, ref min, ref max, attr.MinLimit, attr.MaxLimit);
        position.x += sliderWidth + 4f;
        position.width = fieldWidth;
        max = EditorGUI.FloatField(position, max);

        if (EditorGUI.EndChangeCheck())
        {
            if (isVector2Int)
                property.vector2IntValue = new Vector2Int(Mathf.FloorToInt(min), Mathf.FloorToInt(max));

            else
                property.vector2Value = new Vector2(min, max);

            property.serializedObject.ApplyModifiedProperties();
        }

        EditorGUI.EndProperty();

        EditorGUI.indentLevel = originalIndentLevel;
    }
}