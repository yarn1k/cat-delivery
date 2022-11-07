using UnityEngine;

namespace Core
{
    public static class ColorExtensions
    {
        public static Color WithAlpha(this Color color, float alpha)
        {
            Color result = color;
            result.a = alpha;
            return result;
        }
    }
}
