using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public abstract class AViewVolume : MonoBehaviour
{
    public int priority = 0;
    public AView view;
    public bool isCutOnSwitch = false; 

    protected bool IsActive { get; private set; }

    protected void SetActive(bool isActive)
    {
        if(isCutOnSwitch)
            CameraController.Instance.Cut();

        if (isActive)
            ViewVolumeBlender.Instance.AddVolume(this);
        else
            ViewVolumeBlender.Instance.RemoveVolume(this);
        IsActive = isActive;

    }
    public virtual float ComputeSelfWeight()
    {
        return 1.0f;
    }
}
