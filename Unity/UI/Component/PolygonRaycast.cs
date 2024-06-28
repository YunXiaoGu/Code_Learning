using UnityEngine;
using UnityEngine.UI;

namespace UI.Component
{
    /// <summary>
    /// 不规则点击区域组件
    /// </summary>
    [RequireComponent(typeof(CanvasRenderer))]
    public class PolygonRaycast : MaskableGraphic, ICanvasRaycastFilter
    {
        public Vector2[] points = new Vector2[0];

        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            toFill.Clear();
        }

        public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
        {
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, sp, eventCamera, out var local))
            {
                return false;
            }

            return OverlapPoint(local, points);
        }

        /// <summary>
        /// 点是否在多边形内
        /// </summary>
        /// <para>使用射线法</para>
        /// <remarks>
        /// 如果多边形有重叠区域，则重叠区域是否算作在多边形内取决于规则
        ///     奇-偶规则（Odd-even Rule）
        ///     非零环绕数规则（Nonzero Winding Number Rule）
        /// </remarks>
        bool OverlapPoint(Vector2 point, Vector2[] polygon)
        {
            if (polygon.Length < 3)
            {
                return false;
            }

            int j = polygon.Length - 1;
            bool oddNodes = false;
            for (int k = 0; k < polygon.Length; k++)
            {
                Vector2 point1 = polygon[k];
                Vector2 point2 = polygon[j];
                if ((point1.y > point.y != point2.y > point.y) && (point.x < (point2.x - point1.x) * (point.y - point1.y) / (point2.y - point1.y) + point1.x))
                {
                    oddNodes = !oddNodes;
                }

                j = k;
            }

            return oddNodes;
        }
    }
}