using UnityEngine;

namespace Util
{
    public static class VectorUtil
    {
        /// <summary>
        /// 点是否在多边形内
        /// </summary>
        /// <para>使用射线法</para>
        /// <remarks>
        /// 如果多边形有重叠区域，则重叠区域是否算作在多边形内取决于规则
        ///     奇-偶规则（Odd-even Rule）
        ///     非零环绕数规则（Nonzero Winding Number Rule）
        /// </remarks>
        public static bool OverlapPoint(Vector2 point, Vector2[] polygon)
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

        /// <summary>
        /// 判断AB与CD两条线段是否相交
        /// </summary>
        public static bool IsIntersect(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            Vector3 ab = b - a;
            Vector3 ca = a - c;
            Vector3 cd = d - c;

            Vector3 v1 = Vector3.Cross(ca, cd);

            if (Mathf.Abs(Vector3.Dot(v1, ab)) > 1e-6)
            {
                // 不共面
                return false;
            }

            if (Vector3.Cross(ab, cd).sqrMagnitude <= 1e-6)
            {
                // 平行
                return false;
            }

            Vector3 ad = d - a;
            Vector3 cb = b - c;
            // 快速排斥
            if (Mathf.Min(a.x, b.x) > Mathf.Max(c.x, d.x) || Mathf.Max(a.x, b.x) < Mathf.Min(c.x, d.x)
                                                        || Mathf.Min(a.y, b.y) > Mathf.Max(c.y, d.y) || Mathf.Max(a.y, b.y) < Mathf.Min(c.y, d.y)
                                                        || Mathf.Min(a.z, b.z) > Mathf.Max(c.z, d.z) || Mathf.Max(a.z, b.z) < Mathf.Min(c.z, d.z)
            )
                return false;

            // 跨立试验
            if (Vector3.Dot(Vector3.Cross(-ca, ab), Vector3.Cross(ab, ad)) > 0
                && Vector3.Dot(Vector3.Cross(ca, cd), Vector3.Cross(cd, cb)) > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 叉乘
        /// </summary>
        public static float Cross(Vector2 a, Vector2 b)
        {
            return a.x * b.y - b.x * a.y;
        }

        /// <summary>
        /// 判断AB与CD两条线段是否相交
        /// </summary>
        public static bool IsIntersect(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
        {
            //以线段ab为准，是否c，d在同一侧
            Vector2 ab = b - a;
            Vector2 ac = c - a;
            Vector2 ad = d - a;
            if (Cross(ab, ac) * Cross(ab, ad) >= 0)
            {
                return false;
            }

            //以线段cd为准，是否ab在同一侧
            Vector2 cd = d - c;
            Vector2 ca = a - c;
            Vector2 cb = b - c;
            if (Cross(cd, ca) * Cross(cd, cb) >= 0)
            {
                return false;
            }

            return true;
        }
    }
}