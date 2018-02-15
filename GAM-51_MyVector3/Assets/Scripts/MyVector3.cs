using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyVector3 {
    // Add()
    public static Vector3 Add(Vector3 v3A, Vector3 v3B) {
        return (v3A + v3B);
    }
    public static Vector3 Add(Vector2 v2A, Vector2 v2B) {
        return (v2A + v2B);
    }

    // Subtract()
    public static Vector3 Subtract(Vector3 v3A, Vector3 v3B) {
        return Add(v3A, -v3B);
    }
    public static Vector2 Subtract(Vector2 v2A, Vector2 v2B) {
        return Add(v2A, -v2B);
    }
 
    // Scale()
    public static Vector3 Scale(Vector3 v3, float s) {
        return (v3 * s);
    }
    public static Vector3 Scale(Vector2 v2, float s) {
        return (v2 * s);
    }

    // Magnitude()
    public static float Magnitude(float x, float y, float z) {
        return Mathf.Sqrt( Mathf.Pow(x, 2) + Mathf.Pow(y, 2) + Mathf.Pow(z, 2) );
    }
    public static float Magnitude(Vector3 v3) {
        return Magnitude(v3.x, v3.y, v3.z);
    }

    public static float Magnitude(float x, float y) {
        return Mathf.Sqrt( Mathf.Pow(x, 2) + Mathf.Pow(y, 2) );
    }
    public static float Magnitude(Vector2 v2) {
        return Magnitude(v2.x, v2.y);
    }

    // Normalize()
    public static Vector3 Normalize(Vector3 v3) {
        return (v3 / Magnitude(v3));
    }

    public static Vector3 Normalize(Vector2 v2) {
        return (v2 / Magnitude(v2));
    }

    // DotProduct()
    public static float DotProduct(Vector3 v3A, Vector3 v3B) {
        return ( (v3A.x * v3B.x) + (v3A.y * v3B.y) + (v3A.z * v3B.z) );
    }
    public static float DotProduct(Vector2 v2A, Vector2 v2B) {
        return ( (v2A.x * v2B.x) + (v2A.y * v2B.y) );
    }

    // CrossProduct()
    public static Vector3 CrossProduct(Vector3 v3A, Vector3 v3B) {
        return new Vector3( (v3A.y * v3B.z) - (v3A.z * v3B.y), (v3A.z * v3B.x) - (v3A.x * v3B.z), (v3A.x * v3B.y) - (v3A.y * v3B.x) );
    }
}
