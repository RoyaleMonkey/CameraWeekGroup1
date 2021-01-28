using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curve : MonoBehaviour
{
    
    public Vector3 A,B,C,D = Vector3.zero;

    Vector3 GetPosition(float t) => MathUtils.CubicBezier(A, B, C, D, t);

    public Vector3 GetPosition(float t, Matrix4x4 localToWorldMatrix)
    {
        Vector3 pLocal = GetPosition(t);
        Vector3 pWorld = localToWorldMatrix.MultiplyPoint(pLocal);
        return pWorld;
    }

    void DrawGizmo(Color c, Matrix4x4 localToWorldMatrix)
    {
        Gizmos.color = c;
        Gizmos.DrawSphere(localToWorldMatrix.MultiplyPoint(A),0.25f);
        Gizmos.DrawSphere(localToWorldMatrix.MultiplyPoint(B),0.25f);
        Gizmos.DrawSphere(localToWorldMatrix.MultiplyPoint(C),0.25f);
        Gizmos.DrawSphere(localToWorldMatrix.MultiplyPoint(D),0.25f);
        Vector3 previousPoint = Vector3.zero;
        for (float t = 0; t < 1; t+=0.1f)
        {
            Vector3 point = GetPosition(t, localToWorldMatrix);
            if(previousPoint!=Vector3.zero)
                Gizmos.DrawLine(previousPoint,point);
            previousPoint = point;
        }
        Gizmos.DrawLine(previousPoint,GetPosition(1,localToWorldMatrix));
    }

    private void OnDrawGizmos()
    {
        DrawGizmo(Color.red, transform.localToWorldMatrix);
    }
}
