using System.Collections.Generic;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

namespace UI.Component.Editor
{
    [CustomEditor(typeof(PolygonRaycast))]
    [CanEditMultipleObjects]
    public class PolygonRaycastEditor : Editor
    {
        private PolygonRaycast component;

        private bool canEditCollider; // 是否开启编辑碰撞体
        private List<bool> intersects; // 用于标记相交的线段

        private void OnEnable()
        {
            component = (PolygonRaycast)target;
            intersects = new List<bool>();
        }

        private void OnDisable()
        {
            canEditCollider = false;
            intersects.Clear();
            intersects = null;
        }

        public override void OnInspectorGUI()
        {
            DrawEditCollider();
            base.OnInspectorGUI();
        }

        private void OnSceneGUI()
        {
            DrawCollider();
        }

        // 绘制编辑碰撞体按钮
        private void DrawEditCollider()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Edit Collider", GUILayout.Width(80));
            GUI.backgroundColor = canEditCollider ? Color.green : Color.white;
            if (GUILayout.Button("Edit", GUILayout.Width(60)))
            {
                canEditCollider = !canEditCollider;
                if (canEditCollider)
                {
                    ToolManager.SetActiveTool<NoneTool>();
                    Selection.activeGameObject = component.gameObject;
                }
                else
                {
                    ToolManager.RestorePreviousTool();
                }
            }

            GUI.backgroundColor = Color.white;
            if (canEditCollider)
            {
                if (GUILayout.Button("+", GUILayout.Width(30)))
                {
                    Undo.RecordObject(component, "Edit PolygonRaycast Point");
                    Vector2[] points = component.Points;
                    if (points == null)
                    {
                        points = new Vector2[3];
                    }
                    else
                    {
                        Vector2[] newPoints = new Vector2[points.Length + 1];
                        for (int i = 0; i < points.Length; i++)
                        {
                            newPoints[i] = points[i];
                        }

                        points = newPoints;
                    }
                    component.Points = points;
                }

                if (GUILayout.Button("-", GUILayout.Width(30)))
                {
                    Undo.RecordObject(component, "Edit PolygonRaycast Point");
                   Vector2[] points = component.Points;
                    if (points != null && points.Length > 3)
                    {
                        Vector2[] newPoints = new Vector2[points.Length - 1];
                        for (int i = 0; i < newPoints.Length; i++)
                        {
                            newPoints[i] = points[i];
                        }
                        component.Points = newPoints;
                    }
                }

                if (GUILayout.Button("□", GUILayout.Width(30)))
                {
                    // 将RectTransform的四个角位置赋值给points
                    Undo.RecordObject(component, "Edit PolygonRaycast Points");
                    Vector2[] points = new Vector2[4];
                    RectTransform rectTransform = component.rectTransform;
                    Vector2 size = rectTransform.sizeDelta;
                    Vector2 pivot = rectTransform.pivot;
                    Vector2 offset = new Vector2(size.x * pivot.x, size.y * pivot.y);
                    points[0] = new Vector2(-offset.x, -offset.y);
                    points[1] = new Vector2(-offset.x, size.y - offset.y);
                    points[2] = new Vector2(size.x - offset.x, size.y - offset.y);
                    points[3] = new Vector2(size.x - offset.x, -offset.y);
                    component.Points = points;
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        // 绘制碰撞体
        private void DrawCollider()
        {
            if (component.Points == null || component.Points.Length < 3)
            {
                return;
            }

            Vector2[] points = component.Points;

            intersects.Clear();
            for (int i = 0; i < points.Length; i++)
            {
                intersects.Add(false);
            }

            // 检查相交线段，标记相交的线段
            for (int i = 0; i < points.Length; i++)
            {
                Vector2 p1 = component.transform.TransformPoint(points[i]);
                Vector2 p2 = component.transform.TransformPoint(points[(i + 1) % points.Length]);

                for (int j = i + 1; j < points.Length; j++)
                {
                    Vector2 q1 = component.transform.TransformPoint(points[j]);
                    Vector2 q2 = component.transform.TransformPoint(points[(j + 1) % points.Length]);

                    if (IsIntersect(p1, p2, q1, q2))
                    {
                        intersects[i] = true;
                        intersects[j] = true;
                    }
                }
            }

            var originColor = Handles.color;
            // 绘制多边形
            for (int i = 0; i < points.Length; ++i)
            {
                Vector2 worldPoint = component.transform.TransformPoint(points[i]);
                Vector2 nextWorldPoint = component.transform.TransformPoint(points[(i + 1) % points.Length]);
                Handles.color = intersects[i] ? Color.red : Color.green;
                Handles.DrawLine(worldPoint, nextWorldPoint);
                // Handles.DrawAAPolyLine(1f, new Vector3[] { worldPoint, nextWorldPoint });

                if (canEditCollider)
                {
#if UNITY_2022_1_OR_NEWER
                    Vector2 newWorldPoint = Handles.FreeMoveHandle(worldPoint, HandleUtility.GetHandleSize(component.transform.position) * 0.05f, Vector3.zero, Handles.DotHandleCap);
#else
                    Vector2 newWorldPoint = Handles.FreeMoveHandle(worldPoint, Quaternion.identity, HandleUtility.GetHandleSize(component.transform.position) * 0.05f, Vector3.zero, Handles.DotHandleCap);
#endif
                    if (worldPoint != newWorldPoint)
                    {
                        EditorUtility.SetDirty(component);
                        Undo.RecordObject(component, "Edit PolygonRaycast Collider");
                        points[i] = component.transform.InverseTransformPoint(newWorldPoint);
                        component.Points = points;
                    }

                    // 操作点上的序号，大一点
                    Handles.Label(worldPoint, i.ToString(), new GUIStyle()
                    {
                        fontSize = 20,
                        normal = new GUIStyleState() { textColor = Color.red }
                    });
                }
            }

            Handles.color = originColor;
        }
        
        /// <summary>
        /// 叉乘
        /// </summary>
        float Cross(Vector2 a, Vector2 b)
        {
            return a.x * b.y - b.x * a.y;
        }

        /// <summary>
        /// 判断AB与CD两条线段是否相交
        /// </summary>
        bool IsIntersect(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
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