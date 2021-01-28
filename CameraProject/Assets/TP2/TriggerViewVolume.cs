using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerViewVolume : AViewVolume
{
    private bool isTriggered = false;


    private void OnTriggerStay()
    {
        isTriggered = true;
    }

    private void Update()
    {
        if (isTriggered && !IsActive)
            SetActive(true);
        if(!isTriggered && IsActive)
            SetActive(false);
       
    }

    private void LateUpdate()
    {
        isTriggered = false;
    }

}
