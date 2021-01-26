using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Range(0, 1)] public float configWeight = 0;

    public List<AView> activeViews = new List<AView>();
    public CameraConfiguration activeConfig;
    private Camera cameraComponent = null;

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
        activeConfig = InterpolateView();
        SetConfig(activeConfig);
    }

    public void AddView(AView view) => activeViews.Add(view);

    public void RemoveView(AView view) => activeViews.Remove(view);

    public void SetConfig(CameraConfiguration config)
    {
        if (cameraComponent == null)
            cameraComponent = GetComponent<Camera>();

        Quaternion orientation = Quaternion.Euler(config.pitch, config.Yaw, config.roll);
        transform.rotation = orientation;
        Vector3 offset = orientation * (Vector3.back * config.distance);
        transform.position = config.pivot + offset;
        cameraComponent.fieldOfView = config.fov;
    }

    public CameraConfiguration LerpConfigs(CameraConfiguration configA, CameraConfiguration configB, float configWeight)
    {
        CameraConfiguration lerpedConfig = new CameraConfiguration();

        lerpedConfig.distance = Mathf.Lerp(configA.distance, configB.distance, configWeight);
        lerpedConfig.Yaw = (int) Mathf.Lerp(configA.Yaw, configB.Yaw, configWeight);
        lerpedConfig.pitch = (int) Mathf.Lerp(configA.pitch, configB.pitch, configWeight);
        lerpedConfig.roll = (int) Mathf.Lerp(configA.roll, configB.roll, configWeight);
        lerpedConfig.fov = (int) Mathf.Lerp(configA.fov, configB.fov, configWeight);
        lerpedConfig.pivot = Vector3.Lerp(configA.pivot, configB.pivot, configWeight);
        activeConfig = lerpedConfig;
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
            yaw += config.Yaw*view.weight;
            pitch += config.pitch * view.weight;
            roll += config.roll * view.weight;
            fov += config.fov * view.weight;
            weight += view.weight;
            pivot += config.pivot * view.weight;
        }

        CameraConfiguration InterpolatedConfig = new CameraConfiguration();
        InterpolatedConfig.distance = distance / weight;
        InterpolatedConfig.Yaw = (int) (yaw / weight);
        InterpolatedConfig.pitch = (int) (pitch / weight);
        InterpolatedConfig.roll = (int) (roll / weight);
        InterpolatedConfig.fov = (int) (fov / weight);
        InterpolatedConfig.pivot = pivot / weight;

        return InterpolatedConfig;
    }

    private void OnDrawGizmos()
    {
        activeConfig.DrawGizmos(Color.red);
    }


}
