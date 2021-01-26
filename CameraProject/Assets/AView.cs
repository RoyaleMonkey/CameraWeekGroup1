using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AView : MonoBehaviour
{
    [Range(0,1)]
    public float weight;
    public bool isActiveOnStart;

    public virtual CameraConfiguration GetConfiguration()
    {
        return new CameraConfiguration();
    }

    public void SetActive(bool isActive)
    {
        CameraController.Instance.AddView(this);
    }

    private void Start()
    {
        if(isActiveOnStart)
            SetActive(isActiveOnStart);
    }
}
