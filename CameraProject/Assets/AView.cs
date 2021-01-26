using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AView : MonoBehaviour
{

    public float weight;

    public virtual CameraConfiguration GetConfiguration()
    {
        return new CameraConfiguration();
    }
}
