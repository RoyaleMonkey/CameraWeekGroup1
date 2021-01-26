using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedFollowView : AView
{
    public float roll, fov;

    public Transform target;
    public GameObject centralPoint;
    public float yawOffsetMax;
    public float pitchOffsetMax;

    public override CameraConfiguration GetConfiguration()
    {
        CameraConfiguration config = new CameraConfiguration();

        Vector3 targetDir = (target.position - transform.position).normalized;
        Vector3 centralDir = (centralPoint.transform.position - transform.position).normalized;

        float centralYaw = Mathf.Atan2(centralDir.x, centralDir.z) * Mathf.Rad2Deg;
        float centralPitch = -Mathf.Asin(centralDir.y) * Mathf.Rad2Deg;

        float targetYaw = Mathf.Atan2(targetDir.x, targetDir.z) * Mathf.Rad2Deg;
        float targetPitch = -Mathf.Asin(targetDir.y) * Mathf.Rad2Deg;

        if(Mathf.Abs(centralYaw-targetYaw) < 180)
        {
            config.yaw = Mathf.Atan2(targetDir.x, targetDir.z) * Mathf.Rad2Deg;
        }
        if (Mathf.Abs(centralPitch - centralPitch) < 90)
        {
            config.pitch = -Mathf.Asin(targetDir.y) * Mathf.Rad2Deg;
        }
        config.roll = roll;
        config.fov = fov;
        config.pivot = transform.position;
        config.distance = 0;

        return config;
    }
}
