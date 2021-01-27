using System.Collections;
using System.Collections.Generic;
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
}
