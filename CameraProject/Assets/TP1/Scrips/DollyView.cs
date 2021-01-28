using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollyView : AView
{
    public float roll = 0;
    public float distance = 0;
    public float fov = 60;
    public Transform target = null;
    public Rail rail = null;
    public float railPosition = 0;
    public float speed;
    public bool isAuto = false;

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        if (horizontal != 0)
        {
            railPosition += horizontal * Time.deltaTime * speed;
            if (railPosition < 0)
                railPosition = 0;
            if (!rail.isLoop)
                railPosition = Mathf.Min(railPosition, rail.GetLength());
        }

    }

    public override CameraConfiguration GetConfiguration()
    {
        CameraConfiguration config = new CameraConfiguration();

        config.roll = roll;
        config.distance = distance;
        config.fov = fov;
        config.pivot = isAuto ? rail.findNearestPoint(target.position) : rail.GetPosition(railPosition);
        Vector3 targetDir = (target.position - config.pivot).normalized;
        config.yaw = Mathf.Atan2(targetDir.x, targetDir.z) * Mathf.Rad2Deg;
        config.pitch = -Mathf.Asin(targetDir.y) * Mathf.Rad2Deg;

        return config;
    }
}

