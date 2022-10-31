using System;
using UnityEngine;

namespace Editor
{
    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Field)]
    public sealed class MinMaxSliderAttribute : PropertyAttribute
    {
        public readonly float MinLimit;
        public readonly float MaxLimit;
        public readonly float Width;

        public MinMaxSliderAttribute(float minLimit, float maxLimit, float width = 35f)
        {
            MinLimit = minLimit;
            MaxLimit = maxLimit;
            Width = width;
        }
    }
}