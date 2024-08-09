#if UNITY_EDITOR
using UnityEngine;

public static class E_VisualDebugUtil
{
    // Copyright Belongs to: https://forum.unity.com/threads/debug-drawbox-function-is-direly-needed.1038499/#post-7814979
    public static void DrawWireCube(Vector3 center, Vector3 scale, Quaternion rotation, Color color = default, float duration = 0)
    {
        // create matrix
        Matrix4x4 m = new();
        m.SetTRS(center, rotation, scale);

        var point1 = m.MultiplyPoint(new Vector3(-0.5f, -0.5f, 0.5f));
        var point2 = m.MultiplyPoint(new Vector3(0.5f, -0.5f, 0.5f));
        var point3 = m.MultiplyPoint(new Vector3(0.5f, -0.5f, -0.5f));
        var point4 = m.MultiplyPoint(new Vector3(-0.5f, -0.5f, -0.5f));

        var point5 = m.MultiplyPoint(new Vector3(-0.5f, 0.5f, 0.5f));
        var point6 = m.MultiplyPoint(new Vector3(0.5f, 0.5f, 0.5f));
        var point7 = m.MultiplyPoint(new Vector3(0.5f, 0.5f, -0.5f));
        var point8 = m.MultiplyPoint(new Vector3(-0.5f, 0.5f, -0.5f));

        Debug.DrawLine(point1, point2, color, duration);
        Debug.DrawLine(point2, point3, color, duration);
        Debug.DrawLine(point3, point4, color, duration);
        Debug.DrawLine(point4, point1, color, duration);

        Debug.DrawLine(point5, point6, color, duration);
        Debug.DrawLine(point6, point7, color, duration);
        Debug.DrawLine(point7, point8, color, duration);
        Debug.DrawLine(point8, point5, color, duration);

        Debug.DrawLine(point1, point5, color, duration);
        Debug.DrawLine(point2, point6, color, duration);
        Debug.DrawLine(point3, point7, color, duration);
        Debug.DrawLine(point4, point8, color, duration);
    }

    public static void DrawWireCube(Bounds bounds, Quaternion rotation, Color color = default, float duration = 0)
    {
        DrawWireCube(bounds.center, bounds.size, rotation, color, duration);
    }
}
#endif