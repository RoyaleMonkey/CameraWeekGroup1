using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeFollowView : AView
{
    public float[] pitch = new float[3];
    public float[] roll = new float[3];
    public float[] fov = new float[3];

    public float yaw;
    public float yawSpeed;

    public Transform target;

    public Curve curve;
    public float curvePosition = 0;
    public float curveSpeed;


    public override CameraConfiguration GetConfiguration()
    {
        CameraConfiguration config = new CameraConfiguration();

        float inputVertical = Input.GetAxis("Vertical") * Time.deltaTime * curveSpeed;
        float inputHorizontal = Input.GetAxis("Horizontal") * Time.deltaTime * yawSpeed;

        curvePosition += inputVertical;
        curvePosition = Mathf.Clamp01(curvePosition);
        yaw += inputHorizontal;

        Matrix4x4 matrix4 = Matrix4x4.TRS(target.position, Quaternion.Euler(0, yaw, 0), Vector3.one);

        config.yaw = yaw;
        config.pivot = curve.GetPosition(curvePosition, matrix4);
        config.fov = MathUtils.QuadratiqueBezier(fov[0],fov[1],fov[2],curvePosition);
        config.roll = MathUtils.QuadratiqueBezier(roll[0], roll[1], roll[2],curvePosition);
        config.pitch = MathUtils.QuadratiqueBezier(pitch[0], pitch[1], pitch[2],curvePosition);

        return config;
    }
}
