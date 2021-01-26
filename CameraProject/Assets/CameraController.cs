using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Range(0, 1)] public float configWeight = 0;

    public List<AView> activeViews = new List<AView>();
    public CameraConfiguration targetConfig;
    public CameraConfiguration activeConfig;
    private Camera cameraComponent = null;

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

        if(cameraSpeed * Time.deltaTime < 1)
        {
            activeConfig = activeConfig + (targetConfig - activeConfig) * cameraSpeed * Time.deltaTime;
        }
        else
        {
            activeConfig = config;
        }

        Quaternion orientation = Quaternion.Euler(activeConfig.pitch, activeConfig.yaw, activeConfig.roll);
        transform.rotation = orientation;
        Vector3 offset = orientation * (Vector3.back * activeConfig.distance);
        transform.position = activeConfig.pivot + offset;
        cameraComponent.fieldOfView = activeConfig.fov;
    }

    public CameraConfiguration LerpConfigs(CameraConfiguration configA, CameraConfiguration configB, float configWeight)
    {
        CameraConfiguration lerpedConfig = new CameraConfiguration();

        lerpedConfig.distance = Mathf.Lerp(configA.distance, configB.distance, configWeight);
        lerpedConfig.yaw = Mathf.Lerp(configA.yaw, configB.yaw, configWeight);
        lerpedConfig.pitch = Mathf.Lerp(configA.pitch, configB.pitch, configWeight);
        lerpedConfig.roll = Mathf.Lerp(configA.roll, configB.roll, configWeight);
        lerpedConfig.fov = Mathf.Lerp(configA.fov, configB.fov, configWeight);
        lerpedConfig.pivot = Vector3.Lerp(configA.pivot, configB.pivot, configWeight);
        targetConfig = lerpedConfig;
        return lerpedConfig;
    }

    private CameraConfiguration InterpolateView()
    {
        float distance = 0;
        float yaw = 0;
        float pitch = 0;
        float roll = 0;
        float fov = 0;
        float weight = 0;
        Vector3 pivot = Vector3.zero;

        foreach (var view in activeViews)
        {
            CameraConfiguration config = view.GetConfiguration();
            distance += distance * weight;
            yaw += config.yaw*view.weight;
            pitch += config.pitch * view.weight;
            roll += config.roll * view.weight;
            fov += config.fov * view.weight;
            weight += view.weight;
            pivot += config.pivot * view.weight;
        }

        CameraConfiguration InterpolatedConfig = new CameraConfiguration();
        InterpolatedConfig.distance = distance / weight;
        InterpolatedConfig.yaw = (yaw / weight);
        InterpolatedConfig.pitch = (pitch / weight);
        InterpolatedConfig.roll = (roll / weight);
        InterpolatedConfig.fov = (fov / weight);
        InterpolatedConfig.pivot = pivot / weight;

        return InterpolatedConfig;
    }

    private void OnDrawGizmos()
    {
        targetConfig.DrawGizmos(Color.red);
    }


}
