using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Internal.Filters;
using UnityEngine;

public class Rail : MonoBehaviour
{
    public bool isLoop;
    public List<GameObject> nodes;

    private float length;

    private void Start()
    {
        GetLength();
    }

    public float GetLength()
    {
        length = 0;
        for (int i = 0; i < nodes.Count - 1; i++)
        {
            length += Vector3.Distance(nodes[i].transform.position, nodes[i + 1].transform.position);
        }
        if (isLoop)
            length += Vector3.Distance(nodes[nodes.Count - 1].transform.position, nodes[0].transform.position);
        return length;
    }

    public Vector3 GetPosition(float distance)
    {
        Vector3 position = new Vector3();
        float distanceBetweenPoints = 0;

        if(distance > GetLength())
        {
            if (isLoop)
                distance = distance % GetLength();
            else
                return nodes[nodes.Count - 1].transform.position;
        }

        for(int i = 0; i < nodes.Count - 1; i++)
        {
            distanceBetweenPoints = Vector3.Distance(nodes[i].transform.position, nodes[i + 1].transform.position);
            if (distanceBetweenPoints < distance)
            {
                distance -= distanceBetweenPoints;
            }
            else
            {
                position = Vector3.Lerp(nodes[i].transform.position, nodes[i + 1].transform.position, distance / distanceBetweenPoints);
                return position;
            }
        }

        distanceBetweenPoints = Vector3.Distance(nodes[nodes.Count - 1].transform.position, nodes[0].transform.position);
        position = Vector3.Lerp(nodes[nodes.Count - 1].transform.position, nodes[0].transform.position, distance / distanceBetweenPoints);

        return position;
    }

    private void OnDrawGizmos()
    {
        if(nodes.Count != transform.childCount)
        {
            nodes = new List<GameObject>(transform.childCount);
            for (int i = 0; i < transform.childCount; i++)
            {
                nodes.Add(transform.GetChild(i).gameObject);
            }
        }
        
        if (nodes.Count > 1)
        {
            for(int i = 0; i < nodes.Count - 1; i++)
            {
                Gizmos.DrawLine(nodes[i].transform.position, nodes[i + 1].transform.position);
            }
            if(isLoop)
                Gizmos.DrawLine(nodes[nodes.Count-1].transform.position, nodes[0].transform.position);

        }
    }

    public Vector3 findNearestPoint(Vector3 targetPosition)
    {
        Vector3 closest = Vector3.zero;
        float smalestDistance = Mathf.Infinity;
        foreach (var node in nodes)
        {
            float dist = Vector3.Distance(targetPosition, node.transform.position);
            if (dist < smalestDistance)
            {
                smalestDistance = dist;
                closest = node.transform.position;
            }
        }

        int index = nodes.FindIndex(x => x.transform.position == closest);

        Vector3 pointPrevious;
        if (index == 0 && !isLoop)
            pointPrevious = nodes[0].transform.position;
        else
        {
            Vector3 PreviousNodePosition;
            if (index == 0 && isLoop)
                PreviousNodePosition = nodes[nodes.Count - 1].transform.position;
            else
                PreviousNodePosition = nodes[index - 1].transform.position;

            float distancePrevious = findProjectedDistance(targetPosition, closest, PreviousNodePosition);
            distancePrevious = Mathf.Clamp(distancePrevious, 0, Vector3.Distance(closest, PreviousNodePosition));
            pointPrevious = Vector3.Lerp(closest, PreviousNodePosition, distancePrevious/ Vector3.Distance(closest, PreviousNodePosition));
        }

        Vector3 pointNext;
        if (index == nodes.Count - 1 && !isLoop)
            pointNext = nodes[nodes.Count - 1].transform.position;
        else
        {
            Vector3 NextNodePosition;
            if (index == nodes.Count - 1 && isLoop)
                NextNodePosition = nodes[0].transform.position;
            else 
                NextNodePosition = nodes[index + 1].transform.position;

            float distanceNext = findProjectedDistance(targetPosition, closest, NextNodePosition);
            distanceNext = Mathf.Clamp(distanceNext, 0, Vector3.Distance(closest, NextNodePosition));
            pointNext = Vector3.Lerp(closest, NextNodePosition, distanceNext/ Vector3.Distance(closest, NextNodePosition));
        }


        float targetToPointNext = Vector3.Distance(targetPosition, pointNext);
        float targetToPointPrevious = Vector3.Distance(targetPosition, pointPrevious);

        if (targetToPointPrevious < targetToPointNext)
            return pointPrevious;
        else 
            return pointNext;
    }

    private float findProjectedDistance(Vector3 targetPosition,Vector3 nearestNodePosition, Vector3 otherNodePosition)
    {
        //Vector3 targetNormal = (otherNodePosition - nearestNodePosition).normalized;
        //float projectedDistance = Vector3.Project(targetPosition - nearestNodePosition,targetNormal).magnitude;
        //return projectedDistance;

        Vector3 targetDirection = targetPosition - nearestNodePosition;
        Vector3 otherNodeDiretion = (otherNodePosition - nearestNodePosition).normalized;
        float projectedDistance = (Vector3.Dot(targetDirection, otherNodeDiretion));
                                  
        return projectedDistance;
    }
}
