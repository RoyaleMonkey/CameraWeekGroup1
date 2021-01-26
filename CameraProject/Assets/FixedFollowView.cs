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

        float yawDif = centralYaw - targetYaw;

        if ((yawDif) > 180)
            yawDif -= 360;
        else if (yawDif < -180)
            yawDif += 360;
        if (Mathf.Abs(yawDif) < yawOffsetMax)
        {
            config.yaw = Mathf.Atan2(targetDir.x, targetDir.z) * Mathf.Rad2Deg;
        }
        else
        {
            if (yawDif > 0)
                config.yaw = centralYaw - yawOffsetMax;
            else
                config.yaw = centralYaw + yawOffsetMax;
        }
        if (Mathf.Abs(centralPitch - centralPitch) < pitchOffsetMax)
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
