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
}
