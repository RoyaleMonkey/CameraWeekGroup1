using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerViewVolume : AViewVolume
{
    public GameObject target;


    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == target)
            SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == target)
            SetActive(false);
    }

}
