using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereViewVolume : AViewVolume
{
    public Transform target;
    public float outerRadius;
    public float innerRadius;

    private float distance;

    private void Update()
    {
        distance = Vector3.Distance(transform.position, target.position);

        if (distance <= outerRadius && !IsActive)
            SetActive(true);
        if (distance > outerRadius && IsActive)
            SetActive(false);
    }

    public override float ComputeSelfWeight()
    {
        float weight =  (outerRadius - distance) / (outerRadius - innerRadius);
        weight = Mathf.Clamp01(weight);
        return weight;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, innerRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, outerRadius);
    }
}
