using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Range(0, 1)]
    public float configWeight;

    public List<CameraConfiguration> camConfigs;
    
    private Camera cameraComponent;

    void Awake()
    {
    }

    public void SetConfig(CameraConfiguration config)
    {
        if(cameraComponent == null)
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
        lerpedConfig.Yaw = (int)Mathf.Lerp(configA.Yaw, configB.Yaw, configWeight);
        lerpedConfig.pitch = (int)Mathf.Lerp(configA.pitch, configB.pitch, configWeight);
        lerpedConfig.roll = (int)Mathf.Lerp(configA.roll, configB.roll, configWeight);
        lerpedConfig.fov = (int)Mathf.Lerp(configA.fov, configB.fov, configWeight);
        lerpedConfig.pivot = Vector3.Lerp(configA.pivot, configB.pivot, configWeight);

        return lerpedConfig;
    }

    private void OnValidate()
    {
        SetConfig(LerpConfigs(camConfigs[0], camConfigs[1], configWeight));
    }
}
