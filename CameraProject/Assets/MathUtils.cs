using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathUtils : MonoBehaviour
{

    Vector3 LinearBezier(Vector3 A, Vector3 B, float t)
    {
        return (1 - t) * A + t * B;
    }

    Vector3 QuadratiqueBezier(Vector3 A, Vector3 B, Vector3 C, float t)
    {
        return (1 - t) * LinearBezier(A, B, t) + t * LinearBezier(B, C, t);
    }

    Vector3 CubicBezier(Vector3 A, Vector3 B, Vector3 C,Vector3 D, float t)
    {
        return (1 - t) * QuadratiqueBezier(A, B, C, t) + t * QuadratiqueBezier(B, C, D, t);
    }

}