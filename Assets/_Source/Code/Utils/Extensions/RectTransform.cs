using UnityEngine;

namespace _Client_.Scripts.Utils.Extensions
{
    public static partial class Extensions
    {
        public static void SetHeight(this RectTransform transform, float height)
        {
            transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        }
        
        public static void SetWidth(this RectTransform transform, float width)
        {
            transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        }

        public static void SetOffsetFromScreenBorder(this RectTransform transform, 
                                                     Canvas canvas, Vector3 position, float offsetX)
        {
            var width = Screen.width / canvas.scaleFactor;
            var globalHalfWidth = width * 0.5f;
            var localHalfWidth = transform.rect.width * 0.5f;
            var percentageOffset = width * offsetX;

            if (position.x - localHalfWidth - percentageOffset < -globalHalfWidth)
            {
                position.x = -globalHalfWidth + localHalfWidth + percentageOffset;
            }
            else if (position.x + localHalfWidth + percentageOffset > globalHalfWidth)
            {
                position.x = globalHalfWidth - localHalfWidth - percentageOffset;
            }

            transform.anchoredPosition = position;
        }
    }
}