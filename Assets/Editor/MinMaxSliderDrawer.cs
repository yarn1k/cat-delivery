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

        var attr = (MinMaxSliderAttribute)attribute;

        EditorGUI.LabelField(position, new GUIContent(property.displayName));

        Rect rect = new Rect(EditorGUIUtility.labelWidth, position.y, attr.Width / 2, EditorGUIUtility.singleLineHeight);
        Rect minRect = GetRect(rect, attr.Width, 2f);
        Rect sliderRect = GetRect(minRect, position.width - minRect.x - attr.Width * 2 + 8f);
        Rect maxRect = GetRect(sliderRect, attr.Width);

        float min = isVector2Int ? property.vector2IntValue.x : property.vector2Value.x;
        float max = isVector2Int ? property.vector2IntValue.y : property.vector2Value.y;

        EditorGUI.BeginProperty(position, label, property);
        EditorGUI.BeginChangeCheck();
        min = EditorGUI.FloatField(minRect, min);
        max = EditorGUI.FloatField(maxRect, max);
        EditorGUI.MinMaxSlider(sliderRect, ref min, ref max, attr.MinLimit, attr.MaxLimit);
        if (EditorGUI.EndChangeCheck())
        {
            if (isVector2Int)
                property.vector2IntValue = CalculateIntRange(min, max);

            else
                property.vector2Value = new Vector2(min, max);

            property.serializedObject.ApplyModifiedProperties();
        }
        EditorGUI.EndProperty();
    }

    private Vector2Int CalculateIntRange(float min, float max)
    {
        return new Vector2Int(Mathf.FloorToInt(min), Mathf.FloorToInt(max));
    }
    private Rect GetRect(Rect lastRect, float width, float padding = 5f)
    {
        return new Rect(
            lastRect.position.x + lastRect.width + padding,
            lastRect.position.y,
            width,
            lastRect.height);
    }
}