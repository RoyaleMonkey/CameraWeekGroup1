using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CameraConfiguration
{
    [Range(0,360)]
    public int Yaw;
    [Range(-90, 90)]
    public int pitch;
    [Range(-180, 180)]
    public int roll;

    public Vector3 pivot;

    public float distance;
    [Range (0,180)]
    public int fov;

    public Quaternion GetRotation()
    {
        return Quaternion.Euler(Yaw, pitch, roll);
    }
    public Vector3 GetPosition()
    {
        return (pivot + (GetRotation() * Vector3.back * distance));
    }

    public void DrawGizmos(Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(pivot, 0.25f);
        Vector3 position = GetPosition();
        Gizmos.DrawLine(pivot, position);
        Gizmos.matrix = Matrix4x4.TRS(position, GetRotation(), Vector3.one);
        Gizmos.DrawFrustum(Vector3.zero, fov, 0.5f, 0f, Camera.main.aspect);
        Gizmos.matrix = Matrix4x4.identity;
    }
}
