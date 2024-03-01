using UnityEngine;
using UnityEngine.UI;

namespace _Client_.Scripts.Utils.Extensions
{
    public static partial class Extensions
    {
        public static void SetAlpha(this Image image, float alpha)
        {
            var newColor = image.color;
            newColor.a = alpha;
            image.color = newColor;
        }
        
        public static void SetAlpha(this SpriteRenderer image, float alpha)
        {
            var newColor = image.color;
            newColor.a = alpha;
            image.color = newColor;
        }
    }
}