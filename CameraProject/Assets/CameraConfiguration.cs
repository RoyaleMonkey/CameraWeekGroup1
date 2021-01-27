using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CameraConfiguration
{
    [Range(0,360)]
    public float yaw = 0;
    [Range(-90, 90)]
    public float pitch = 0;
    [Range(-180, 180)]
    public float roll = 0;

    public Vector3 pivot = Vector3.zero;

    public float distance = 0;
    [Range (0,180)]
    public float fov = 0;

    public Quaternion GetRotation()
    {
        return Quaternion.Euler(pitch, yaw, roll);
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

    public static CameraConfiguration operator +(CameraConfiguration a, CameraConfiguration b)
    {
        CameraConfiguration newConfig = new CameraConfiguration();
        newConfig.yaw = a.yaw + b.yaw;
        newConfig.pitch = a.pitch + b.pitch;
        newConfig.roll = a.roll + b.roll;
        newConfig.distance = a.distance + b.distance;
        newConfig.fov = a.fov + b.fov;
        newConfig.pivot = a.pivot + b.pivot;

        return newConfig;
    }

    public static CameraConfiguration operator -(CameraConfiguration a, CameraConfiguration b)
    {
        CameraConfiguration newConfig = new CameraConfiguration();
        newConfig.yaw = a.yaw - b.yaw;
        newConfig.pitch = a.pitch - b.pitch;
        newConfig.roll = a.roll - b.roll;
        newConfig.distance = a.distance - b.distance;
        newConfig.fov = a.fov - b.fov;
        newConfig.pivot = a.pivot - b.pivot;

        return newConfig;
    }


    public static CameraConfiguration operator *(CameraConfiguration a, float b)
    {
        CameraConfiguration newConfig = new CameraConfiguration();
        newConfig.yaw = a.yaw * b;
        newConfig.pitch = a.pitch * b;
        newConfig.roll = a.roll * b;
        newConfig.distance = a.distance * b;
        newConfig.fov = a.fov * b;
        newConfig.pivot = a.pivot * b;

        return newConfig;
    }

    public static CameraConfiguration operator /(CameraConfiguration a, float b)
    {
        CameraConfiguration newConfig = new CameraConfiguration();
        newConfig.yaw = a.yaw / b;
        newConfig.pitch = a.pitch / b;
        newConfig.roll = a.roll / b;
        newConfig.distance = a.distance / b;
        newConfig.fov = a.fov / b;
        newConfig.pivot = a.pivot / b;

        return newConfig;
    }
}
