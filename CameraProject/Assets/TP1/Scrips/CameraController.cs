using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public List<AView> activeViews = new List<AView>();
    public CameraConfiguration targetConfig;
    public CameraConfiguration activeConfig;
    private Camera cameraComponent = null;
    private bool isCutRequested = false;

    public float cameraSpeed = 2.0f;

    #region Singleton

    private static CameraController instance;

    public static CameraController Instance
    {
        get => instance;
    }

    #endregion

    private void Awake()
    {
        if (instance)
            Destroy(gameObject);
        else
            instance = this;
    }

    private void Update()
    {
        targetConfig = InterpolateView();
        SetConfig(targetConfig);


    }

    public void AddView(AView view) => activeViews.Add(view);

    public void RemoveView(AView view) => activeViews.Remove(view);

    public void Cut() => isCutRequested = true;

    public void SetConfig(CameraConfiguration config)
    {

        if(activeConfig == null)
        {
            if (activeViews.Count < 1)
                return;

            activeConfig = config;
        }

        if (cameraComponent == null)
            cameraComponent = GetComponent<Camera>();

        if(cameraSpeed * Time.deltaTime < 1 && !isCutRequested)
        {
            float yawDif = targetConfig.yaw - activeConfig.yaw;
            if (Mathf.Abs(yawDif) > 180)
            {
                if (activeConfig.yaw > 0)
                    activeConfig.yaw -= 360;
                else
                    activeConfig.yaw += 360;
            }
            activeConfig = activeConfig + (targetConfig - activeConfig) * cameraSpeed * Time.deltaTime;
        }
        else
        {
            activeConfig = config;
            isCutRequested = false;
        }

        Quaternion orientation = Quaternion.Euler(activeConfig.pitch, activeConfig.yaw, activeConfig.roll);
        transform.rotation = orientation;
        Vector3 offset = orientation * (Vector3.back * activeConfig.distance);
        transform.position = activeConfig.pivot + offset;
        cameraComponent.fieldOfView = activeConfig.fov;
    }

    //public CameraConfiguration LerpConfigs(CameraConfiguration configA, CameraConfiguration configB, float configWeight)
    //{
    //    CameraConfiguration lerpedConfig = new CameraConfiguration();

    //    lerpedConfig.distance = Mathf.Lerp(configA.distance, configB.distance, configWeight);
    //    lerpedConfig.yaw = Mathf.Lerp(configA.yaw, configB.yaw, configWeight);
    //    lerpedConfig.pitch = Mathf.Lerp(configA.pitch, configB.pitch, configWeight);
    //    lerpedConfig.roll = Mathf.Lerp(configA.roll, configB.roll, configWeight);
    //    lerpedConfig.fov = Mathf.Lerp(configA.fov, configB.fov, configWeight);
    //    lerpedConfig.pivot = Vector3.Lerp(configA.pivot, configB.pivot, configWeight);
    //    targetConfig = lerpedConfig;
    //    return lerpedConfig;
    //}

    private CameraConfiguration InterpolateView()
    {
        CameraConfiguration newConfig = new CameraConfiguration();
        float weight = 0;
        Vector2 sum = Vector2.zero;

        foreach (var view in activeViews)
        {
            CameraConfiguration config = view.GetConfiguration();
            newConfig += config*view.weight;
            weight += view.weight;

            sum += new Vector2(Mathf.Cos(config.yaw * Mathf.Deg2Rad),
                Mathf.Sin(config.yaw * Mathf.Deg2Rad)) * view.weight;
        }

        newConfig.yaw = Vector2.SignedAngle(Vector2.right, sum);

        if (weight != 0)
            newConfig /= weight;
        else
            newConfig = targetConfig;

        return newConfig;
    }

    private void OnDrawGizmos()
    {
        activeConfig.DrawGizmos(Color.red);
    }


}
