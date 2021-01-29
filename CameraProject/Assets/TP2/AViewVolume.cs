using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AViewVolume : MonoBehaviour
{
    public int priority = 0;
    public AView view;

    protected bool IsActive { get; private set; }

    protected void SetActive(bool isActive)
    {
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